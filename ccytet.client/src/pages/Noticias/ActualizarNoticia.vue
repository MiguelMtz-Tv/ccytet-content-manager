<template>
  <div class="dialog">
    <div class="dialog-header">
      <span class="text-lg font-bold">
        Editar noticia "{{ titulo }}"
      </span>
    </div>
    <div class="dialog-body">
      <div class="w-full flex">
        <div class="w-3/4 mr-0.5">
          <div class="mb-3">Titulo</div>
          <a-input v-model:value="titulo" class="w-full mb-4" placeholder="Ingresa el titulo" />
        </div>
        <div class="w-1/4 ml-0.5">
          <div class="mb-3">Autor</div>
          <a-input v-model:value="autor" class="w-full mb-4" placeholder="Nombre del autor (opcional)" />
        </div>
      </div>

      <ckeditor id="editor" :editor="editor" v-model="body" :config="editorConfig"></ckeditor>

      <div class="mt-4">
        <div class="mb-3">Cargar imagenes</div>
        <a-upload v-model:file-list="fileList" :before-upload="bfUpload" list-type="picture-card"
          @preview="handlePreview" @remove="onRemove">
          <div v-if="fileList!.length < 8">
            <plus-outlined />
            <div style="margin-top: 8px">Añadir</div>
          </div>
        </a-upload>
        <a-modal :open="previewVisible" :title="previewTitle" :footer="null" @cancel="handleCancel">
          <img alt="example" style="width: 100%" :src="previewImage" />
        </a-modal>
      </div>
    </div>
    <div class="dialog-footer">
      <a-button type="primary" ghost class="mr-2" @click="onSubmit">Actualizar</a-button>
      <a-button type="primary" ghost @click="$emit('closeDialog')">Cerrar</a-button>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { NoticiasService } from '@/services/noticias-service';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import type { UploadProps } from 'ant-design-vue';
import type { AxiosResponse } from 'axios';
import { onMounted, ref, type Ref } from 'vue';
import { Upload, notification } from 'ant-design-vue';
import { Server } from '@/libraries/servers';
import {CustomNotification} from '@/services/custom-notification-service'
import router from '@/routing';

let _noticiasService: NoticiasService = new NoticiasService()
let _notifications: CustomNotification = new CustomNotification()

let spinning: Ref<boolean> = ref(false)
const props = defineProps(['id'])
const emits = defineEmits(['closeDialog'])

/* Editables */
let titulo  : Ref<string>   = ref('')
let autor   : Ref<string>   = ref('')
let body    : Ref<string>   = ref('')

const editor = ClassicEditor
const editorConfig = {
}

onMounted(() => {
    _noticiasService.watch(props.id)
        .then((res: AxiosResponse) => {
            let data = res.data
            if (data.session && data.action) {
                let view      = data.result.view

                autor.value   = view.autor,
                titulo.value  = view.titulo,
                body.value    = view.texto

                let imgs: Array<string> = data.result.images
                imgs.forEach((e : string, i: number) => {
                    let uid = e.split('\\')[2]
                    fileList.value!.push({
                        uid: String(uid),
                        name: uid,
                        status: 'done',
                        url: Server.baseUrl + e
                    })

                    files.push({
                       uid: String(uid),
                       base64: 'exist'
                    })
                }) 
            } else {
                notification.error({
                    message: data.message
                })
            }
        })
        .catch((error) => {
            notification.error({
                message: error.message,
            })
        })
})

const onSubmit = () => {
    if(validateForm()){
      spinning.value = true
      _noticiasService.update({
        idNoticia      : props.id,
        titulo  : titulo.value,
        autor   : autor.value,
        body    : body.value,
        files   : files
      })
      .then((res : AxiosResponse) => {
        spinning.value = false
        if(res.data.session && res.data.action){
          emits('closeDialog')
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

function validateForm(): boolean {
  let validation = [
    { name: 'title', valid: titulo.value.trim() != '' },
    { name: 'body', valid: body.value.trim() != '' },
    { name: 'pics', valid: files.length > 0 },
  ]

  if (validation.every(e => e.valid)) {
    return true
  } else {
    let errors = validation.filter(e => !e.valid)
    errors.forEach(e => {
      switch (e.name) {
        case 'title':
          _notifications.error('El campo Titulo es obligatorio')
          break
        case 'body':
          _notifications.error('No hay nada escrito en el cuerpo de la noticia')
          break
        case 'pics':
          _notifications.error('Es necesario subir al menos una imagen')
          break
      }
    })

    return false
  }
}

/* Configuración de carga de imagenes */

const previewVisible    = ref(false);
const previewImage      = ref('');
const previewTitle      = ref('');

const fileList = ref<UploadProps['fileList']>([]);
let files: Array<any>   = new Array<any>()

const handleCancel = () => {
    previewVisible.value = false;
    previewTitle.value = '';
};

let images: any = []

function getBase64(file: File) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
}

const handlePreview = async (file: UploadProps['fileList'][number]) => {
    if (!file.url && !file.preview) {
        file.preview = (await getBase64(file.originFileObj)) as string;
    }
    previewImage.value = file.url || file.preview;
    previewVisible.value = true;
    previewTitle.value = file.name || file.url.substring(file.url.lastIndexOf('/') + 1);
}

const onRemove = (element : any) => {
    let index = files.findIndex(e => e.uid == element.uid)
    files.splice(index, 1)
}

const bfUpload = async (e : any) => {
    const valid = e.type == 'image/png' || e.type == 'image/jpeg'
    if(valid){
      let base64: any
      await getBase64(e).then(e1 => base64 = e1)

      files.push({
        uid: e.uid,
        base64: base64
      })
      return false
    }

    return Upload.LIST_IGNORE
}
</script>