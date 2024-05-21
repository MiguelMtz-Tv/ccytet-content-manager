<template>
    <!-- Dialogs -->
    <v-dialog fullscreen v-model="details">
        <VerConvocatoria :id="selected" @close-dialog="details = false"></VerConvocatoria>
    </v-dialog>

    <v-dialog fullscreen v-model="update">
        <ActualizarConvocatoria :id="selected" @close-dialog="update = false" @updated="index()">
        </ActualizarConvocatoria>
    </v-dialog>

    <div class="flex space-x-2">
        <a-input-search v-model:value="txtSearch" placeholder="Buscar" style="width: 200px" @search="onSearch" />
        <a-tooltip>
            <template #title>
                <span>Filtrar desde</span>
            </template>
            <input type="date" v-model="dateFrom" @change="index()"
                class="text-xs border rounded p-0.5 focus:outline-none">
        </a-tooltip>
        <a-tooltip>
            <template #title>
                <span>Filtrar Hasta</span>
            </template>
            <input type="date" v-model="dateTo" @change="index()"
                class="text-xs border rounded p-0.5 focus:outline-none">
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
    <v-data-table-virtual fixed-header :loading="loading" :headers="headers" :items="items" :height="height"
        item-value="id">
        <template v-slot:item.portada="{item}">
            <img class="h-8" :src="baseUrl+item.portadaPath" alt="">
        </template>
        <template v-slot:item.acciones="{item}">
            <a-dropdown :trigger="'click'">
                <div class="border rounded flex items-center justify-center p-0.5 cursor-pointer hover:bg-gray-100"
                    @click.prevent>
                    <span class="mr-1 text-xs">Acciones</span>
                    <DownOutlined />
                </div>
                <template #overlay>
                    <a-menu>
                        <a-menu-item @click="openDetails(item.idConvocatoria)">Ver en detalle</a-menu-item>
                        <a-menu-item @click="openUpdate(item.idConvocatoria)">Editar</a-menu-item>
                        <a-menu-item @click="toggleVisibility(item.idConvocatoria)">{{ item.eliminado ? 'Hacer visible' : 'Hacer invisible' }}</a-menu-item>
                        <a-menu-item @click="toggleStatus(item.idConvocatoria)">{{ item.abierto ? 'Cerrar convocatoria' : 'Abrir convocatoria' }}</a-menu-item>
                    </a-menu>
                </template>
            </a-dropdown>
        </template>

        <template v-slot:item.abierto="{ item }">
            <table class="w-[130px]">
                <tbody>
                    <tr>
                        <td class="p-1 border">
                            <div :class="[item.abierto ? 'breadcrum-green' : 'breadcrum-red', 'm-auto text-center']">
                                {{ item.abierto ? 'Abierta' : 'Cerrada' }}
                            </div>
                        </td>
                        <td class="p-1 border">
                            <div :class="[item.eliminado ? 'breadcrum-red' : 'breadcrum-green', 'm-auto text-center']">
                                {{ item.eliminado ? 'Invisible' : 'Visible' }}
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </template>
    </v-data-table-virtual>
    <a-pagination v-model:current="page" @change="managePagination" :total="count" />
</template>

<script setup lang="ts">
import { type Ref, ref } from 'vue';
import { Server } from '@/libraries/servers';
import { ConvocatoriasService } from '@/services/convocatorias-service';
import { onMounted } from 'vue';
import {CloseOutlined} from '@ant-design/icons-vue';
import VerConvocatoria from './VerConvocatoria.vue';
import ActualizarConvocatoria from './ActualizarConvocatoria.vue'
import { notification } from 'ant-design-vue';

/* INITIALIZATION */
onMounted(() => {
    index()
})

/* SERVICES */
const _convocatoriasService: ConvocatoriasService = new ConvocatoriasService()

/* UTILS */
let loading         : Ref<boolean>  = ref(false)
let details         : Ref<boolean>  = ref(false)
let update          : Ref<boolean>  = ref(false)
let baseUrl         : string        = Server.baseUrl
let selected        : Ref<string>   = ref('')


/* PAGINATIONS */
let count           : Ref<number>   = ref(0)
let length          : Ref<number>   = ref(10)
let pages           : Ref<number>   = ref(1)
let page            : Ref<number>   = ref(1)

const managePagination = (epage : any, elength: any) => {
    page.value = epage
    length.value = elength
    index()
}

let dateFrom        : Ref<string>   = ref('')
let dateTo          : Ref<string>   = ref('')

let height: Ref<number> = ref(window.innerHeight - 158)
window.addEventListener('resize', () => {
    height.value = window.innerHeight - 158
})

/* 
* DataSource section
*/
let txtSearch: Ref<string> = ref('')

const index = () => {
    loading.value = true
    _convocatoriasService.dataSource({
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

const onSearch = () => {
    index()
}

const headers : Array<any> = [
    {title: '',                     align: 'start',     key: 'portada'},
    {title: 'Acciones',             align: 'start',     key: 'acciones'},
    {title: 'Estatus',              align: 'start',     key: 'abierto'},
    {title: 'Titulo',               align: 'start',     key: 'titulo'},
    {title: 'Fecha de creación',    align: 'start',     key: 'fechaCreacion'},
    {title: 'Creado por',           align: 'start',     key: 'userCreatorName'},
    {title: 'Ultima actualización', align: 'start',     key: 'fechaActualizacion'},
    {title: 'Actualizado por',      align: 'start',     key: 'userUpdaterName'},
]
let items: Array<any> = new Array<any>()

/* 
* Botón de opciones
*/

const openUpdate = (id : string) => {
    update.value = true
    selected.value = id
}

const openDetails = (id : string) => {
    details.value = true
    selected.value = id
}

const toggleStatus = (id : string) => {
    _convocatoriasService.toggleStatus(id)
    .then(res => {
        let data = res.data
        if(data.session && data.action){
            index()
            notification.success({
                message: 'Se actualizó el estatus de la convocatoria',
            })
        }else{
            notification.error({
                message: 'No se actualizó el estatus de la convocatoria',
                description: data.message
            })
        }
    })
    .catch(error => {
        notification.success({
            message: 'Error de conexión al servidor',
            description: error.message
        })
    })
}

const toggleVisibility = (id : string) => {
    _convocatoriasService.toggleVisibility(id)
    .then(res => {
        let data = res.data
        if(data.session && data.action){
            index()
            notification.success({
                message: 'Se actualizó el estatus de la convocatoria',
            })
        }else{
            notification.error({
                message: 'No se actualizó el estatus de la convocatoria',
                description: data.message
            })
        }
    })
    .catch(error => {
        notification.success({
            message: 'Error de conexión al servidor',
            description: error.message
        })
    })
}

</script>