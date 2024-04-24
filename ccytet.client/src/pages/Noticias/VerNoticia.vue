<template>
    <div class="w-full h-full bg-white flex flex-col">
        <div class="font-bold text-lg p-4">
            {{ noticia.titulo }}
        </div>
        <div class="flex-grow p-4 overflow-auto">
            <a-carousel arrows>
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
                <img :src="baseUrl + image" alt="" v-for="image in images">
            </a-carousel>
            
            <div class="w-full text-center">
                {{ noticia.autor }}
            </div>

            <div v-html="noticia.texto"></div>
        </div>
        <div class="p-4 flex justify-end">
            <a-button type="primary" danger ghos @click="$emit('close-dialog')">Cerrar</a-button>
        </div>
    </div>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted, ref, type Ref } from 'vue';
import { NoticiasService } from '@/services/noticias-service';
import { type AxiosResponse } from 'axios';
import { Server } from '@/libraries/servers';
import { LeftCircleOutlined, RightCircleOutlined } from '@ant-design/icons-vue';


const _noticiasService = new NoticiasService();
const baseUrl = Server.baseUrl

const props = defineProps(['id'])
let noticia: Ref<any> = ref({})
let images: Ref<Array<string>> = ref(new Array<string>())

onMounted(() => {
    _noticiasService.watch(props.id)
    .then((res : AxiosResponse) => {
        let data = res.data
        if(data.session && data.action){
            console.log(data)
            noticia.value = data.result.view
            images.value = data.result.images
        }else{

        }
    })
    .catch((error : any) => {

    })
})

onUnmounted(() =>{
    console.log('componente cerrado')
})

</script>