<template>
    <div class="dialog">
        <div class="dialog-header">
            <span class="text-lg font-bold">
                Editar componente
            </span>
        </div>
        <div class="dialog-body">
            
        </div>
        <div class="dialog-footer">
            <a-button type="primary" ghost class="mr-2">Actualizar</a-button>
            <a-button type="primary" ghost  @click="$emit('close-dialog')">Cerrar</a-button>
        </div>
    </div>
</template>

<script lang="ts" setup>
import { NoticiasService } from '@/services/noticias-service';
import type { AxiosResponse } from 'axios';
import { onMounted } from 'vue';

let _noticiasService: NoticiasService = new NoticiasService()
const props = defineProps(['id'])

onMounted(() => {
    _noticiasService.watch(props.id)
    .then((res : AxiosResponse) => {
        let data = res.data
        if(data.session && data.action){
            console.log(data.result)
        }else{
            console.log(data)
        }
    })
    .catch((error) => {
        console.log(error)
    })
})
</script>