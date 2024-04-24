<template>
  <a-spin :spinning="spinning" tip="Subiendo noticia...">
    <div class="mb-4 w-full flex justify-between items-center">
        <div><span class="text-lg font-bold">Noticias</span> / <span class="text-blue-500">Crer noticia</span></div>
        <div>
            <a-button type="primary" ghost @click="submit()">Publicar noticia</a-button>
        </div>
    </div>

    <div class="w-full flex">
        <div class="w-3/4 mr-0.5">
          <div class="mb-3">Titulo</div>
          <a-input v-model:value="titulo" class="w-full mb-4" placeholder="Ingresa el titulo"/>
        </div>
        <div class="w-1/4 ml-0.5">
          <div class="mb-3">Autor</div>
          <a-input v-model:value="autor" class="w-full mb-4" placeholder="Nombre del autor (opcional)"/>
        </div>
    </div>

    <ckeditor id="editor" :editor="editor" v-model="body" :config="editorConfig"></ckeditor>

    <div class="mt-4">
        <div class="mb-3">Cargar imagenes</div>
        <a-upload
          v-model:file-list="fileList"
          :before-upload="bfUpload"
          list-type="picture-card"
          @preview="handlePreview"
          @remove="onRemove"
          >
          <div v-if="fileList!.length < 8">
            <plus-outlined />
            <div style="margin-top: 8px">Añadir</div>
          </div>
        </a-upload>
        <a-modal :open="previewVisible" :title="previewTitle" :footer="null" @cancel="handleCancel">
          <img alt="example" style="width: 100%" :src="previewImage" />
        </a-modal>
    </div>
    
  </a-spin>
</template>
  

<script setup lang="ts">
  import { type Ref,  } from 'vue'
  import ClassicEditor from '@ckeditor/ckeditor5-build-classic'
  import { PlusOutlined } from '@ant-design/icons-vue';
  import { ref } from 'vue';
  import type { UploadProps } from 'ant-design-vue';
  import { NoticiasService } from '@/services/noticias-service';
  import { notification } from 'ant-design-vue';
import type { AxiosRequestConfig, AxiosResponse } from 'axios';
import router from '@/routing';


  const _noticiasService = new NoticiasService()

  let titulo: Ref<string> = ref('')
  let body: Ref<string> = ref('')
  let autor: Ref<string> = ref('')

  let spinning: Ref<boolean> = ref(false)

  // Configura el editor con el módulo de carga de imágenes
  const editor = ClassicEditor
  const editorConfig = {
  }

  function submit(){
    if(validateForm()){
      spinning.value = true
      _noticiasService.create({
        titulo  : titulo.value,
        autor   : autor.value,
        body    : body.value,
        files   : files
      })
      .then((res : AxiosResponse) => {
        spinning.value = false
        if(res.data.session && res.data.action){
          router.push('/noticias')
          notification.success({
            message: 'Noticia creada',
            description: 'Ahora puedes verla en la lista de noticias'
          })
        }else{
          notification.error({
            message: 'No se creó la noticia',
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

  function getBase64(file: File) {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result);
      reader.onerror = error => reject(error);
    });
  }

  const bfUpload = async (e: any) => {
    let base64: any
    await getBase64(e).then(e1 => base64 = e1)

    files.push({
      uid: e.uid,
      base64: base64
    })

    return false
  }


  function validateForm(): boolean{
    let validation = [
      { name: 'title', valid: titulo.value.trim() != '' },
      { name: 'body', valid: body.value.trim() != '' },
      { name: 'pics', valid: files.length > 0 },
    ]

    if(validation.every(e => e.valid)){
      return true
    }else{
      let errors = validation.filter(e => !e.valid)
      errors.forEach(e => {
        switch(e.name){
          case 'title':
            setNotification('El campo Titulo es obligatorio')
            break
          case 'body':
            setNotification('No hay nada escrito en el cuerpo de la noticia')
            break
          case 'pics':
            setNotification('Es necesario subir al menos una imagen')
            break
        }
      })

      return false
    }
  }

  function setNotification(message : string){
    notification.open({message: message, placement: 'bottomRight', style:{
      'border' : 'red solid 1px',
      'background' : '#fff2f0',
    }})
  }

  function onRemove(event : any){
    let index = files.findIndex(e => e.uid == event.uid)
    files.splice(index, 1)
  }

  // Configurar carga de imagenes
  const previewVisible = ref(false);
  const previewImage = ref('');
  const previewTitle = ref('');

  const fileList = ref<UploadProps['fileList']>([]);
  let files: Array<any> = new Array<any>()

  const handleCancel = () => {
    previewVisible.value = false;
    previewTitle.value = '';
  };

  let images: any = []

  const handlePreview = async (file: UploadProps['fileList'][number]) => {
    if (!file.url && !file.preview) {
      let base64 = await getBase64(file.originFileObj)
      file.preview = (base64) as string;
    }
    previewImage.value = file.url || file.preview;
    previewVisible.value = true;
    previewTitle.value = file.name || file.url.substring(file.url.lastIndexOf('/') + 1);
  };
</script>

<style>
.ck-editor__editable_inline {
    min-height: 300px;
}
</style>