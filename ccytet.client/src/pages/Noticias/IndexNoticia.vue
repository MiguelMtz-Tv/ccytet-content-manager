<template>
    <!-- Dialogs -->
    <v-dialog fullscreen v-model="details">
        <VerNoticia :id="selectedNew" @close-dialog="details = false"></VerNoticia>
    </v-dialog>

    <v-dialog fullscreen v-model="update">
        <ActualizarNoticia :id="selectedNew" @closeDialog="update = false; index()"></ActualizarNoticia>
    </v-dialog>

    <div class="flex space-x-2">
        <a-input-search v-model:value="txtSearch" placeholder="Buscar" style="width: 200px" @search="onSearch" />
        <a-tooltip>
            <template #title>
                <span>Filtrar desde</span>
            </template>
            <input type="date" v-model="dateFrom" @change="index()" class="text-xs border rounded p-0.5 focus:outline-none">
        </a-tooltip>
        <a-tooltip>
            <template #title>
                <span>Filtrar Hasta</span>
            </template>
            <input type="date" v-model="dateTo" @change="index()" class="text-xs border rounded p-0.5 focus:outline-none">
        </a-tooltip>
        <a-tooltip>
            <template #title>
                <span>Limpiar fechas</span>
            </template>
            <button class="btn-basic text-xs" @click="dateTo=''; dateFrom=''; index()">
                <CloseOutlined />
            </button>
        </a-tooltip>
    </div>
    <v-data-table-virtual fixed-header :loading="loading" :headers="headers" :items="items" :height="height" item-value="id">
        <template v-slot:item.portada="{item}">
            <img class="h-8" :src="baseUrl+item.portada" alt="">
        </template>
        <template v-slot:item.eliminado="{item}">
            <span class="breadcrum-red" v-if="item.eliminado">Invisible</span>
            <span class="breadcrum-green" v-else>Visible</span>
        </template>
        <template v-slot:item.acciones="{item}">
            <a-dropdown :trigger="'click'">
                <div class="border rounded flex items-center justify-center p-0.5 cursor-pointer hover:bg-gray-100" @click.prevent>
                    <span class="mr-1 text-xs">Acciones</span>
                    <DownOutlined />
                </div>
                <template #overlay>
                    <a-menu>
                        <a-menu-item @click="openUpdate(item.idNoticia)">Editar</a-menu-item>
                        <a-menu-item @click="openDetails(item.idNoticia)">Ver en detalle</a-menu-item>
                        <a-menu-item @click="toggleVisibility(item.idNoticia)">{{ item.eliminado ? 'Hacer visible' : 'Hacer invisible' }}</a-menu-item>
                    </a-menu>
                </template>
            </a-dropdown>
        </template>
    </v-data-table-virtual>
    <a-pagination v-model:current="page" @change="managePagination" :total="count" />
</template>

<script setup lang="ts">
import { Server } from '@/libraries/servers';
import { NoticiasService } from '@/services/noticias-service';
import { onMounted } from 'vue';
import { type Ref, ref } from 'vue';
import { DownOutlined } from '@ant-design/icons-vue';
import VerNoticia from './VerNoticia.vue'
import ActualizarNoticia from './ActualizarNoticia.vue';
import {CloseOutlined} from '@ant-design/icons-vue';
import { notification } from 'ant-design-vue';

onMounted(() => {
    index()
})

const _noticiasService = new NoticiasService()
/* BOOLEANS */
let details         : Ref<boolean>  = ref(false)
let update          : Ref<boolean>  = ref(false)

/* PAGINATIONS */
let count           : Ref<number>   = ref(0)
let length          : Ref<number>   = ref(10)
let pages           : Ref<number>   = ref(1)
let page            : Ref<number>   = ref(1)

let dateFrom        : Ref<string>   = ref('')
let dateTo          : Ref<string>   = ref('')

//ELEMENTS
let selectedNew     : Ref<string>   = ref('')
let updateElement   : Ref<any>      = ref(null)

let txtSearch       : Ref<string>   = ref('')

const onSearch = (e : any) => {
    index()
}

const openDetails = (id : string) => {
    details.value = true
    selectedNew.value = id
}

const openUpdate = (id : string) => {
    update.value = true
    selectedNew.value = id
}

const toggleVisibility = (id : string) => {
    _noticiasService.toggleVisibility(id)
    .then((res) => {
        let data = res.data
        if(data.session && data.action){
            index()
            notification.success({
                message: 'Se cambió el estatus de la noticia',
            })
        }else{
            notification.error({
                message: 'No se cambió el estatus de la noticia',
                description: data.message
            })
        }
    })
    .catch(error => {
        notification.error({
                message: 'Error de conexión',
                description: error.message
            })
    })
}

const baseUrl = Server.baseUrl

let loading: Ref<boolean> = ref(false)

const index = () => {
    loading.value = true
    _noticiasService.dataSource({
        dateFrom: dateFrom.value,
        dateTo: dateTo.value,
        page: page.value,
        length: length.value,
        search: txtSearch.value.trim(),
    }).then(res => {
        loading.value = false
        let data = res.data
        if(data.session && data.action){
            items = data.result.rows
            count = data.result.count
            length = data.result.length
            pages = data.result.pages
            page = data.result.page
        }
    }).catch(error => {
        loading.value = false
        console.log(error)
    })
}

let height: Ref<number> = ref(window.innerHeight - 158)
window.addEventListener('resize', () => {
    height.value = window.innerHeight - 158
})

const managePagination = (epage : any, elength: any) => {
    page.value = epage
    length.value = elength
    index()
}

const headers : Array<any> = [
    {title: '',                     align: 'start',     key: 'portada'},
    {title: 'Acciones',             align: 'start',     key: 'acciones'},
    {title: 'Titulo',               align: 'start',     key: 'titulo'},
    {title: 'Estatus',              align: 'start',     key: 'eliminado'},
    {title: 'Autor',                align: 'start',     key: 'autor'},
    {title: 'Fecha de creación',    align: 'start',     key: 'fechaCreacion'},
    {title: 'Creado por',           align: 'start',     key: 'userCreatorName'},
    {title: 'Ultima actualización', align: 'start',     key: 'fechaActualizacion'},
    {title: 'Actualizado por',      align: 'start',     key: 'userUpdaterName'},
]
let items: Array<any> = new Array<any>()
</script>

<style>
.ant-input-search-button {
    display: flex;
    justify-content: center;
    align-items: center;
}

tr {
    font-size: 11px;
    max-width: 300px;
    white-space: nowrap;
    overflow: hidden; /* Oculta el texto que no cabe */
    text-overflow: ellipsis;
}
</style>


