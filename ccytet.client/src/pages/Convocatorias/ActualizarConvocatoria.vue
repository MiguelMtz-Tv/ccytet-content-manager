<template>
  <div class="dialog">
    <div class="dialog-header">
      <div>
        <span class="text-lg font-bold">Convocatoria</span> / <span class="text-blue-500">Actualizarconvocatoria</span>
      </div>
    </div>

    <div class="dialog-body">

      <div class="mb-3">Titulo</div>
      <div class="w-full flex justify-between">
        <a-input v-model:value="titulo" class="w-full mb-4" placeholder="Ingresa el titulo" />
      </div>

      <div class="mb-3">Portada</div>
      <div class="flex w-full justify-between mb-1">
        <input type="file" class="files-none" @change="logFile" ref="portadaInput" :multiple="false" accept="image/*">
        <button class="btn-basic" @click="portadaInput.value = ''; portada = ''">Quitar imagen</button>
      </div>
      <div class="w-full flex items-center justify-center border overflow-auto h-[248px] mb-3">
        <div class=" max-h-[240px] max-w-[648px] bg-gray-50">
          <img :src="portada">
        </div>
      </div>

      <div class="mb-3">Descripción</div>
      <ckeditor id="editor" :editor="editor" v-model="body"></ckeditor>

      <div class="my-3">Archivos de la convocatoria</div>
      <div>
        <a-upload-dragger @remove="onRemove" name="file" :multiple="true" v-model:file-list="files"
          :before-upload="beforeUpload">
          <p class="ant-upload-drag-icon"> <inbox-outlined></inbox-outlined> </p>
          <p class="ant-upload-text">Click o arrastra los archivos</p>
        </a-upload-dragger>
      </div>
    </div>

    <div class="dialog-footer">
      <a-button type="primary" ghost class="mr-2" @click="onsubmit()">Actualizar</a-button>
      <a-button type="primary" ghost @click="emits('close-dialog')">Cancelar</a-button>
    </div>
  </div>
</template>

<script setup lang="ts">
import ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import { ref, type Ref, onMounted } from 'vue';
import { InboxOutlined } from '@ant-design/icons-vue';
import { ConvocatoriasService } from '@/services/convocatorias-service'
import { CustomNotification }  from '@/services/custom-notification-service'
import Utils from '@/libraries/utils';
import router from '@/routing';
import { notification, Upload, type UploadProps } from 'ant-design-vue';
import type { AxiosResponse } from 'axios';
import { Server } from '@/libraries/servers';

/* 
*-----------------------------
| Elementos del DOM
*-----------------------------
*/

const portadaInput  : Ref<any>                            = ref(null)

/* 
*----------------------------
| Inicialización de variables
*----------------------------
*/
const props = defineProps(['id'])
const emits = defineEmits(['close-dialog', 'updated'])
const baseUrl = Server.baseUrl
const _convocatoriasService: ConvocatoriasService = new ConvocatoriasService()

let fileList        : Array<{uid:string, base64:string, name: string }>  = []; 
let files           : Ref<UploadProps['fileList']>        = ref<UploadProps['fileList']>([])
let portada         : Ref<string>                         = ref('') 
let titulo          : Ref<string>                         = ref('')
let body            : Ref<string>                         = ref('')

let spinning        : Ref<boolean>                        = ref(false)
let convocatoria    : Ref<any>

let previousPortadaPath!: string

onMounted(() => {
  _convocatoriasService.watch(props.id)
  .then((res : AxiosResponse) => {
    let data = res.data
    if(data.session && data.action){
      previousPortadaPath = baseUrl + data.result.portadaPath
      portada.value       = previousPortadaPath
      titulo.value        = data.result.titulo
      body.value          = data.result.texto
      
      data.result.filesArray.forEach((e : string) => {
        let uid: string = e.split('\\')[3]
        files.value!.push({
          uid: uid,
          name: uid,
          url: e,
          status: 'done'
        })

        fileList.push({
          uid: uid,
          name: uid,
          base64: 'exist'
        })
      });
    }else{
      notification.error({
        message: 'No fue posible cargar los datos de la convocatoria',
        description: data.message
      })
    }
  })
})

const onsubmit = () => {
  let data = {
    idConvocatoria: props.id,
    files         : fileList,
    portada       : portada.value == previousPortadaPath ? null : portada.value,
    titulo        : titulo.value,
    body          : body.value
  }

  if(validateForm()){
      spinning.value = true
      _convocatoriasService.update(data)
      .then((res : AxiosResponse) => {
        spinning.value = false
        if(res.data.session && res.data.action){
          emits('updated')
          emits('close-dialog')
          notification.success({
            message: 'Convocatoria actualizada',
            description: 'Ahora puedes verla en la lista de noticias'
          })
        }else{
          notification.error({
            message: 'No se actualizó la convocatoria',
            description: res.data.message
          })
        }
      })
      .catch(error => {
        notification.error({
          message: 'No actualizó la convocatoria',
          description: error.message
        })
      })
    }
}
  
/* 
*---------------------------------------------
| Configuración de carga de archivos y editor
*---------------------------------------------
*/
const logFile = async (e : any) => {
  let file: File = e.target.files[0]
  portada.value = await Utils.getBase64(file)
}

const beforeUpload = async (e: any) => {
  fileList.push({
    uid: e.uid,
    name: e.name,
    base64: await Utils.getBase64(e)
  })
  return false
}

const onRemove = (e : any) => {
  let index: number = fileList.findIndex(x => x.uid == e.uid)
  fileList.splice(index, 1)
}

// Titulo
const editor = ClassicEditor

/* 
*---------------------------------------------
| Validación del formulario
*---------------------------------------------
*/

const customNotification: CustomNotification = new CustomNotification()

const validateForm = (): boolean => {
    let validation = [
      { name: 'title',    valid: titulo.value.trim()  != '' },
      { name: 'body',     valid: body.value.trim()    != '' },
      { name: 'portada',  valid: portada.value.trim() != '' },
    ]

    if(validation.every(e => e.valid)){
      return true
    }else{
      let errors = validation.filter(e => !e.valid)
      errors.forEach(e => {
        switch(e.name){
          case 'title':
            customNotification.error('El campo Titulo es obligatorio')
            break
          case 'body':
          customNotification.error('No hay nada escrito en el cuerpo de la convocatoria')
            break
          case 'portada':
          customNotification.error('No se ha cargado una imagen de portada')
            break
        }
      })

      return false
    }
  }

</script>
<style>
  .ck-powered-by__label, .ck-icon .ck-reset_all-excluded{
    display: none !important;
  }
</style>