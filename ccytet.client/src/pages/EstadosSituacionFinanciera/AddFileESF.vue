<template>
    <div class="dialog">
        <div class="dialog-header">
            <div>
                <span class="text-lg font-bold">Estados de situación financiera</span> / <span class="text-blue-500">{{ periodo }}</span>
            </div>
        </div>
        <div class="dialog-body">
            <a-upload-dragger @remove="onRemove" name="file" :multiple="true" :before-upload="beforeUpload">
                <p class="ant-upload-drag-icon"> <inbox-outlined></inbox-outlined> </p>
                <p class="ant-upload-text">Click o arrastra los archivos</p>
            </a-upload-dragger>
            <div v-if="error.visible" class="p-3 border-2 border-red-500 bg-red-200 rounded text-red-500 mt-3">
                {{ error.message }}
            </div>
        </div>
        <div class="dialog-footer">
            <a-button type="primary" ghost class="mr-2" @click="submit()">Crear</a-button>
            <a-button type="primary" ghost @click="emits('close-dialog', {created: false, year: periodo.split(' ')[1]})">Cerrar</a-button>
        </div>
    </div>
</template>

<script lang="ts" setup>
import Utils from '@/libraries/utils';
import { onUnmounted, ref, type Ref } from 'vue'
import { InboxOutlined } from '@ant-design/icons-vue';
import { Upload } from 'ant-design-vue';
import { EsfService } from '@/services/esf-service';

const _esfService: EsfService = new EsfService()

const emits = defineEmits(['close-dialog'])
const props = defineProps(['id', 'periodo'])

const fileList : Array<{uid:string, base64:string, name: string}>  = [];
let error: Ref<{visible: boolean, message: string}> = ref({visible: false, message: ''})

const beforeUpload = async (e: any) => {
  const valid = e.type == 'application/pdf'
  if(valid){
        fileList.push({
            uid: e.uid,
            name: e.name,
            base64: await Utils.getBase64(e)
        })
        return false
    }
    showError('Formato de archivo no permitido (debe ser PDF)')
    return Upload.LIST_IGNORE
}

const onRemove = (e : any) => {
  let index: number = fileList.findIndex(x => x.uid == e.uid)
  fileList.splice(index, 1)
}

const showError  = (message : string) => {
    error.value.visible = true
    error.value.message = message
    setTimeout(() => {
        error.value.visible = false
        error.value.message = ''
    }, 3000)
}

const submit = () => {
    if(fileList.length > 0){
        _esfService.addFiles(props.id, fileList)
        .then(res => {
            let data = res.data
            if(data.session && data.action){
                emits('close-dialog', {created: true, year: props.periodo.split(' ')[1]})
            }else{
                showError('No fue posible añadir los archivos: ' + data.message)
            }
        })
    }else{
        showError('No has añadido ningún archivo')
    }
}
</script>