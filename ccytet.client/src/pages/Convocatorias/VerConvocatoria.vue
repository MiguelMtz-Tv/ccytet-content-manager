<template>
    <a-spin :spinning="spinning">
        <div class="w-full h-screen bg-gray-800 flex flex-col">
            <div class="flex-grow w-screen overflow-auto max-w-[700px] m-auto p-4 bg-white">
                <div class="font-bold text-lg">
                    {{ convocatoria.titulo }}
                </div>
                <!-- Imagen de portada -->
                <img :src="baseUrl + convocatoria.portadaPath" alt="">

                <div class="border-y p-2 flex w-full justify-between mt-2">
                    <div>
                        <span :class="convocatoria.abierta ? 'breadcrum-green' : 'breadcrum-red'">
                            {{ convocatoria.abierta ? 'Convocatoria abierta' : 'Convocatoria cerrada' }}
                        </span>
                    </div>
                    <div>
                        {{ convocatoria.fechaCreacion }}
                    </div>
                </div>

                <div>
                    <div v-html="convocatoria.texto"></div>
                    <span class="font-semibold" v-if="files.length > 0">Recursos: </span>
                    <ul>
                        <li v-for="item in files">
                            <a :href="baseUrl + item.path" target="_blank" class="link-a">
                                {{ item.name }}
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="p-4 flex justify-end w-screen max-w-[700px] m-auto bg-white">
                <a-button type="primary" ghost  @click="$emit('close-dialog')">Cerrar</a-button>
            </div>
        </div>
    </a-spin>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted, ref, type Ref } from 'vue';
import { ConvocatoriasService } from '@/services/convocatorias-service';
import { type AxiosResponse } from 'axios';
import { Server } from '@/libraries/servers';


const _convocatoriasService = new ConvocatoriasService();
const baseUrl = Server.baseUrl

const props = defineProps(['id'])

let spinning: Ref<boolean> = ref(false)
let convocatoria: Ref<any> = ref({})
let files: Array<{name: string, path: string}> = new Array<{name: string, path: string}>()

onMounted(() => {
    spinning.value = true
    _convocatoriasService.watch(props.id)
    .then((res : AxiosResponse) => {
        console.log(res)
        spinning.value = false
        let data = res.data

        if(data.session && data.action){
            console.log(data)
            convocatoria.value = data.result
            data.result.filesArray.forEach((e : string) => {
                files.push({
                    name: e.split('\\')[3],
                    path: e
                })
            })
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