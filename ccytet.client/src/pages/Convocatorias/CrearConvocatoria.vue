<template>
  <div class="mb-4 w-full flex justify-between items-center">
    <div>
      <span class="text-lg font-bold">Convocatoria</span> / <span class="text-blue-500">Crer convocatoria</span>
    </div>
    <div>
      <a-button type="primary" ghost @click="onsubmit">Publicar convocatoria</a-button>
    </div>
  </div>

  <div class="mb-3">Titulo</div>
  <div class="w-full flex justify-between">
    <a-input v-model:value="titulo" class="w-full mb-4" placeholder="Ingresa el titulo" />
  </div>

  <div class="mb-3">Portada</div>
  <div class="flex w-full justify-between mb-1">
    <input type="file" class="files-none" @change="logFile" ref="portadaInput" :multiple="false" accept="image/*">
    <button class="btn-basic" @click="portadaInput.value=''; portada=''">Quitar imagen</button>
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
    <a-upload-dragger @remove="onRemove" name="file" :multiple="true" :before-upload="beforeUpload">
      <p class="ant-upload-drag-icon"> <inbox-outlined></inbox-outlined> </p>
      <p class="ant-upload-text">Click o arrastra los archivos</p>
    </a-upload-dragger>
  </div>
</template>

<script setup lang="ts">
import ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import { ref, type Ref } from 'vue';
import { InboxOutlined } from '@ant-design/icons-vue';
import { ConvocatoriasService } from '@/services/convocatorias-service.ts'
import { CustomNotification }  from '@/services/custom-notification-service'
import Utils from '@/libraries/utils';
import router from '@/routing';
import { notification } from 'ant-design-vue';
import type { AxiosResponse } from 'axios';

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

const _convocatoriasService: ConvocatoriasService = new ConvocatoriasService()

const fileList      : Array<{uid:string, base64:string, name: string}>  = []; 
let portada         : Ref<string>                         = ref('') 
let titulo          : Ref<string>                         = ref('')
let body            : Ref<string>                         = ref('')

let spinning        :Ref<boolean>                         = ref(false)

const onsubmit = () => {
  let data = {
    files   : fileList,
    portada : portada.value,
    titulo  : titulo.value,
    body    : body.value
  }

  if(validateForm()){
      spinning.value = true
      _convocatoriasService.create(data)
      .then((res : AxiosResponse) => {
        spinning.value = false
        if(res.data.session && res.data.action){
          router.push('/convocatorias')
          notification.success({
            message: 'Convocatoria creada',
            description: 'Ahora puedes verla en la lista de noticias'
          })
        }else{
          notification.error({
            message: 'No se creó la convocatoria',
            description: res.data.message
          })
        }
      })
      .catch(error => {
        notification.error({
          message: 'No se creó la noticia',
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
  console.log(e)
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