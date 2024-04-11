<template>
    <v-data-table-virtual
    fixed-header
    :loading="loading"
    :headers="headers"
    :items="items"
    :height="height"
    item-value="id"
    density="compact">
    </v-data-table-virtual>
    <a-pagination v-model:current="page" @change="managePagination" :total="count"/>
</template>

<script setup lang="ts">
import NewCard from '@/components/NewCard.vue';
import { NoticiasService } from '@/services/noticias-service';
import { onMounted } from 'vue';
import { type Ref, ref, computed, watch, type ComputedRef } from 'vue';

onMounted(() => {
    index(page.value, length.value)
})

const _noticiasService = new NoticiasService()

let count: Ref<number> = ref(0)
let length: Ref<number> = ref(10)
let pages: Ref<number> = ref(1)
let page: Ref<number> = ref(1)

let loading: Ref<boolean> = ref(false)

function index(epage: number, elength: number){
    loading.value = true
    _noticiasService.dataSource({
        page: epage,
        length: elength,
        search: ''
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

let height: Ref<number> = ref(window.innerHeight - 135)
window.addEventListener('resize', () => {
    height.value = window.innerHeight - 135
})

function managePagination(epage : any, elength: any){
    index(epage, elength)
}

const headers : Array<any> = [
    {title: 'Titulo',               align: 'start',     key: 'titulo'},
    {title: 'Autor',                align: 'start',     key: 'autor'},
    {title: 'Fecha de creación',    align: 'start',     key: 'fechaCreacionNatural'},
    {title: 'Ultima actualización', align: 'start',     key: 'fechaActualizacionNatural'},
]
let items: Array<any> = new Array<any>()
</script>


