<template>
    <a-spin :spinning="spinning">
        <div class="w-full h-screen bg-gray-800 flex flex-col">
            <div class="flex-grow w-screen overflow-auto max-w-[700px] m-auto p-4 bg-white">
                <div class="font-bold text-lg">
                    {{ noticia.titulo }}
                </div>
                <a-carousel arrows class="max-w-[700px] m-auto overflow-auto rounded border">
                    <template #prevArrow>
                        <div class="custom-slick-arrow" style="left: 10px; z-index: 1; color: white;">
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                                stroke="currentColor" class="w-6 h-6">
                                <path stroke-linecap="round" stroke-linejoin="round" d="M15.75 19.5 8.25 12l7.5-7.5" />
                            </svg>
                        </div>
                    </template>
                    <template #nextArrow>
                        <div class="custom-slick-arrow" style="right: 10px; color: white;">
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                                stroke="currentColor" class="w-6 h-6">
                                <path stroke-linecap="round" stroke-linejoin="round" d="m8.25 4.5 7.5 7.5-7.5 7.5" />
                            </svg>
                        </div>
                    </template>

                    <img :src="baseUrl + image" v-for="image in images" class="!h-fit">
                </a-carousel>

                <div class="border-y p-2 text-right mt-2">
                   {{ noticia.fechaCreacion }}
                </div>

                <div v-html="noticia.texto"></div>
            </div>
            <div class="p-4 flex justify-end w-screen max-w-[700px] m-auto bg-white">
                <a-button type="primary" ghost  @click="$emit('close-dialog')">Cerrar</a-button>
            </div>
        </div>
    </a-spin>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted, ref, type Ref } from 'vue';
import { NoticiasService } from '@/services/noticias-service';
import { type AxiosResponse } from 'axios';
import { Server } from '@/libraries/servers';


const _noticiasService = new NoticiasService();
const baseUrl = Server.baseUrl

const props = defineProps(['id'])

let spinning: Ref<boolean> = ref(false)
let noticia: Ref<any> = ref({})
let images: Ref<Array<string>> = ref(new Array<string>())

onMounted(() => {
    spinning.value = true
    _noticiasService.watch(props.id)
    .then((res : AxiosResponse) => {
        spinning.value = false
        let data = res.data

        if(data.session && data.action){
            console.log(data)
            noticia.value = data.result.view
            images.value = data.result.images
        }else{

        }
    })
    .catch((error : any) => {
        spinning.value = false
    })
})

onUnmounted(() =>{
    console.log('componente cerrado')
})

</script>