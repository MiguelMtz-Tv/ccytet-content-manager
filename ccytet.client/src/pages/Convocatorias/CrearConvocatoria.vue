<template>
    <div class="mb-4 w-full flex justify-between items-center">
        <div><span class="text-lg font-bold">Convocatoria</span> / <span class="text-blue-500">Crer convocatoria</span></div>
        <div>
            <a-button type="primary" ghost>Publicar convocatoria</a-button>
        </div>
    </div>

    <div class="mb-3">Titulo</div>
    <div class="w-full flex justify-between">
        <a-input :v-model="titulo" class="w-full mb-4" placeholder="Ingresa el titulo"/>
    </div>

    <div class="mb-3">Portada</div>
    <div>
        <a-upload-dragger
            v-model:fileList="fileList"
            name="file"
            :multiple="false"
            action="https://www.mocky.io/v2/5cc8019d300000980a055e76"
            @change="handleChangeOne"
            @drop="handleDropOne">
            <div class="flex w-full items-center justify-center">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-6 h-6">
                    <path stroke-linecap="round" stroke-linejoin="round" d="m2.25 15.75 5.159-5.159a2.25 2.25 0 0 1 3.182 0l5.159 5.159m-1.5-1.5 1.409-1.409a2.25 2.25 0 0 1 3.182 0l2.909 2.909m-18 3.75h16.5a1.5 1.5 0 0 0 1.5-1.5V6a1.5 1.5 0 0 0-1.5-1.5H3.75A1.5 1.5 0 0 0 2.25 6v12a1.5 1.5 0 0 0 1.5 1.5Zm10.5-11.25h.008v.008h-.008V8.25Zm.375 0a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Z" />
                </svg>
            </div>
            <p class="ant-upload-text">Selecciona un archivo de portada</p>
        </a-upload-dragger>
    </div>

    <div class="mb-3">Descripción</div>
    <ckeditor id="editor" :editor="editor" v-model="editorData" :config="editorConfig"></ckeditor>

    <div class="my-3">Archivos de la convocatoria</div>
    <div>
        <a-upload-dragger
            v-model:fileList="fileList"
            name="file"
            :multiple="true"
            action="https://www.mocky.io/v2/5cc8019d300000980a055e76"
            @change="handleChange"
            @drop="handleDrop">
            <p class="ant-upload-drag-icon"> <inbox-outlined></inbox-outlined> </p>
            <p class="ant-upload-text">Click o arrastra los archivos</p>
        </a-upload-dragger>
    </div>
</template>

<script setup lang="ts">
import ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import { ref } from 'vue';
import { InboxOutlined } from '@ant-design/icons-vue';
import { message } from 'ant-design-vue';
import type { UploadChangeParam } from 'ant-design-vue';

const fileList = ref([]);

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

function handleDrop(e: DragEvent) {
  console.log(e);
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