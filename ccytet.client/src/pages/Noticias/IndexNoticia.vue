<template>
    <v-data-table-virtual
        fixed-header
        :headers="headers"
        :items="items"
        height="500"
        item-value="id"
        density="compact"
    >
        <template v-slot:item.id="{item}">
            {{ item.nombre }}
        </template>
    </v-data-table-virtual>
    <a-pagination v-model:current="current" @change="onchange($event)" :total="500" />
</template>

<script setup lang="ts">
import NewCard from '@/components/NewCard.vue';
import { type Ref, ref } from 'vue';

let page: Ref<number> = ref(1)
const current = ref(6);

function onchange(e : any){
    console.log(e)
}

interface Persona{
    id: number
    nombre: string
    edad: number
}
const headers : Array<any> = [
    {title: 'id', align: 'start', key: 'id'},
    {title: 'nombre', align: 'start', key: 'nombre'},
    {title: 'edad', align: 'start', key: 'edad'},
]
let items: Array<Persona> = new Array<Persona>()

for(let i: number = 0; i < 50; i++){
    items.push({
        id: i + 1,
        nombre: 'nombre ' + i,
        edad: 20
    })
}
</script>


