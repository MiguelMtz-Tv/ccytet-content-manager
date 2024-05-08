<template>
    <!-- Dialogs -->
    <v-dialog fullscreen v-model="details">
        <VerNoticia :id="selectedNew" @close-dialog="details = false"></VerNoticia>
    </v-dialog>

    <v-dialog fullscreen v-model="update">
        <ActualizarNoticia :id="selectedNew" @close-dialog="update = false"></ActualizarNoticia>
    </v-dialog>

    <div class="flex space-x-2">
        <a-input-search v-model:value="txtSearch" placeholder="Buscar" style="width: 200px" @search="onSearch" />
        <a-tooltip>
            <template #title>
                <span>Filtrar desde</span>
            </template>
            <input type="date" class="text-xs border rounded p-0.5 focus:outline-none">
        </a-tooltip>
        <a-tooltip>
            <template #title>
                <span>Filtrar Hasta</span>
            </template>
            <input type="date" class="text-xs border rounded p-0.5 focus:outline-none">
        </a-tooltip>
    </div>
    <v-data-table-virtual fixed-header :loading="loading" :headers="headers" :items="items" :height="height" item-value="id">
        <template v-slot:item.portada="{item}">
            <img class="h-8" :src="baseUrl+item.portada" alt="">
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
                        <a-menu-item>Hacer visible</a-menu-item>
                        <a-menu-item @click="openDetails(item.idNoticia)">Ver en detalle</a-menu-item>
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

onMounted(() => {
    index(page.value, length.value)
})

const _noticiasService = new NoticiasService()

let details: Ref<boolean> = ref(false)
let update: Ref<boolean> = ref(false)
let selectedNew: Ref<string> = ref('')

let count: Ref<number> = ref(0)
let length: Ref<number> = ref(10)
let pages: Ref<number> = ref(1)
let page: Ref<number> = ref(1)

//elements
let updateElement: Ref<any> = ref(null)

let txtSearch: Ref<string> = ref('')
function onSearch(e : any){
    index(page.value, length.value)
}

function openDetails(id : string){
    details.value = true
    selectedNew.value = id
}

function openUpdate(id : string){
    update.value = true
    selectedNew.value = id
}

const baseUrl = Server.baseUrl

let loading: Ref<boolean> = ref(false)

function index(epage: number, elength: number){
    loading.value = true
    _noticiasService.dataSource({
        page: epage,
        length: elength,
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

function managePagination(epage : any, elength: any){
    index(epage, elength)
    page.value = epage
    length.value = elength
}

const headers : Array<any> = [
    {title: '',                     align: 'start',     key: 'portada'},
    {title: 'Acciones',                     align: 'start',     key: 'acciones'},
    {title: 'Titulo',               align: 'start',     key: 'titulo'},
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


