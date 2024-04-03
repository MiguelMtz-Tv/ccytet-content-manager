<template>
    <div class="weather-component">
        Bienvenid@ <span class="font-bold">{{name}}</span>
    </div>
</template>

<script setup lang="ts">
import { Sessions } from '@/libraries/sessions';
import { type Ref, ref } from 'vue';

const name: Ref<string> = ref(Sessions.getItem('nombre'))
</script>

<style scoped>
th {
    font-weight: bold;
}
tr:nth-child(even) {
    background: #F2F2F2;
}

tr:nth-child(odd) {
    background: #FFF;
}

th, td {
    padding-left: .5rem;
    padding-right: .5rem;
}

.weather-component {
    text-align: center;
}

table {
    margin-left: auto;
    margin-right: auto;
}
</style>