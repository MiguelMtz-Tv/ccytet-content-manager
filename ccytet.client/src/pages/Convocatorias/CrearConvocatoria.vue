<template>
  <div class="mb-4 w-full flex justify-between items-center">
    <div><span class="text-lg font-bold">Convocatoria</span> / <span class="text-blue-500">Crer convocatoria</span>
    </div>
    <div>
      <a-button type="primary" ghost>Publicar convocatoria</a-button>
    </div>
  </div>
  <div></div>
  <div class="mb-3">Titulo</div>
  <div class="w-full flex justify-between">
    <a-input :v-model="titulo" class="w-full mb-4" placeholder="Ingresa el titulo" />
  </div>

  <div class="mb-3">Portada</div>
  <div class="flex w-full justify-between mb-1">
    <input type="file" class="files-none" @change="logFile" ref="portadaInput" :multiple="false" accept="image/*">
    <button class="btn-basic" @click="portadaInput.value=''; portada=''">Quitar imagen</button>
  </div>
  <div class="w-full flex items-center justify-center border overflow-auto h-[248px]">
    <div class=" max-h-[240px] max-w-[648px] bg-gray-50">
      <img :src="portada">
    </div>
  </div>
 
  <div class="mb-3">Descripción</div>
  <ckeditor id="editor" :editor="editor" v-model="editorData" :config="editorConfig"></ckeditor>

  <div class="my-3">Archivos de la convocatoria</div>
  <div>
    <a-upload-dragger v-model:fileList="fileList" name="file" :multiple="true" :before-upload="beforeUpload" @change="handleChange" @drop="handleDrop($event)">
      <p class="ant-upload-drag-icon"> <inbox-outlined></inbox-outlined> </p>
      <p class="ant-upload-text">Click o arrastra los archivos</p>
    </a-upload-dragger>
  </div>
</template>

<script setup lang="ts">
import ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import { ref, type Ref, computed } from 'vue';
import { InboxOutlined } from '@ant-design/icons-vue';
import { Empty, message } from 'ant-design-vue';
import type { UploadChangeParam } from 'ant-design-vue';

const fileList = ref([]); 
let portada: Ref<string> = ref('') 
const portadaInput: Ref<any> = ref(null)

const logFile = (e : any) => {
  let file: File = e.target.files[0]

  let base64: Promise<string | ArrayBuffer | null> = new Promise((resolve, reject) => {
    let reader: FileReader = new FileReader()
    reader.readAsDataURL(file)
    reader.onload = () => resolve(reader.result)
    reader.onerror = () => reject()
  })

  base64.then((b64) => portada.value = String(b64))
  console.log(portadaInput)
}

const handleChange = (info: UploadChangeParam) => {
  const status = info.file.status;
  if (status !== 'uploading') {
    console.log(info.file, info.fileList);
  }
  if (status === 'done') {
    message.success(`${info.file.name} file uploaded successfully.`);
  } else if (status === 'error') {
    message.error(`${info.file.name} file upload failed.`);
  }
};

function handleDropOne(e: DragEvent) {
  console.log(e);
  return false
}

const handleChangeOne = (info: UploadChangeParam) => {
  const status = info.file.status;
  if (status !== 'uploading') {
    console.log(info.file, info.fileList);
  }
  if (status === 'done') {
    message.success(`${info.file.name} file uploaded successfully.`);
  } else if (status === 'error') {
    message.error(`${info.file.name} file upload failed.`);
  }
};

function handleDrop(file: File) {
  let fileReader: FileReader = new FileReader()
  fileReader.readAsDataURL(file)
}

const beforeUpload = (e: any) => {
  return false
}

// Titulo
const titulo = ref('')

// Configura el editor con el módulo de carga de imágenes
const editor = ClassicEditor
const editorData: any = ''
const editorConfig = {}


</script>

<style>
.ck-editor__editable_inline {
    min-height: 300px;
}

</style>
