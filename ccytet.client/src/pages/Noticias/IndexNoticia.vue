<template>
    <table>
        <thead>
            <th>id</th>
            <th>nombre</th>
            <th>edad</th>
        </thead>
        <tbody>
            <tr>
                <td></td>
                <td></td>
                <td></td>
            </tr>
        </tbody>
    </table>
</template>

<script setup lang="ts">
import NewCard from '@/components/NewCard.vue';
interface Persona{
    id: number
    nombre: string
    edad: number
}
let data: Array<Persona> = new Array<Persona>()

for(let i: number = 0; i < 100; i++){
    data.push({
        id: i + 1,
        nombre: 'nombre ' + i,
        edad: 20
    })
}
</script>


