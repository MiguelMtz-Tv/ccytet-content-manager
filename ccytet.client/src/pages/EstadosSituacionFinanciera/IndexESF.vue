<template>
    <v-dialog v-model="addFiles" width="500px">
      <AddFileESF :id="selectedPeriodo.id" :periodo="selectedPeriodo.name" @close-dialog="addFiles = false"></AddFileESF>
    </v-dialog>
    <div class="mb-2">
      <span class="text-lg font-bold">Estados de situación financiera</span>
    </div>
    <div class="flex">
        <div class="max-w-[300px] min-w-[220px] border mr-2 overflow-auto">
            <div class="p-1 mb-3 mx-auto w-fit flex space-x-2">
              <a-date-picker picker="month" v-model:value="date"/>
              <button class="btn-basic text-sm" @click="btnCreate()">Añadir</button>
            </div>
            <a-tree show-icon :tree-data="treeData">
              <template #icon="{ dataRef }">
                <template v-if="dataRef.level === 0 && dataRef.children.length === 0">
                  <div class="relative">
                    <FolderOutlined class="absolute right-1 top-1.5"/>
                  </div>
                </template>
              </template>
              <template #title="{ dataRef }">
                  <a-dropdown :trigger="['click']">
                    <a @click.prevent>
                      {{ dataRef.title }}
                    </a>
                    <template #overlay>
                      <a-menu>
                        <a-menu-item key="0" @click="openAddFiles(dataRef.id, dataRef.key)">
                          <a>Añadir documento</a>
                        </a-menu-item>
                        <a-menu-item key="1">
                          <a>Eliminar</a>
                        </a-menu-item>
                      </a-menu>
                    </template>
                  </a-dropdown>
              </template>
            </a-tree>
        </div>
        <iframe class="border p-2 rounded bg-gray-100" src="http://localhost:5177/file.pdf" width="100%"
            :height="height+'px'">
            Este navegador no soporta iframes.
        </iframe>
    </div>
</template>
<script lang="ts" setup>
import { ref, type Ref, onMounted  } from 'vue';
import { notification, type TreeProps } from 'ant-design-vue';
import type { Dayjs } from 'dayjs';
import { EsfService } from '@/services/esf-service';
import { FolderOutlined } from '@ant-design/icons-vue';
import AddFileESF from './AddFileESF.vue';

/*
* SERVICES
*/
let _esfService: EsfService = new EsfService()
let selectedPeriodo: Ref<{id: string, name: string}> = ref({id: '', name: ''})

/*
* INITIALIZATION
*/
let addFiles: Ref<boolean> = ref<boolean>(false)
let date: Ref<Dayjs | undefined> = ref<Dayjs>()

const showLine = ref<boolean>(true);;
let treeData = ref<TreeProps['treeData']>([]);
let meses: Array<string> = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre']

/*
* METHODS
*/
onMounted(() => {
  index()
})

const index = () => {
  let year: number = 2024

  _esfService.index(year)
  .then(res => {
    let data = res.data
    if(data.session &&  data.action){
      treeData.value = data.result
    }else{
      notification.error({
        message: 'No fue posible cargar los datos',
        description: data.message
      })
    }
  }
  )
}

let height: Ref<number> = ref(window.innerHeight - 158)
window.addEventListener('resize', () => {
    height.value = window.innerHeight - 100
})

const btnCreate = () => {
  let year: number = date.value!.year()
  let month: number = (date.value?.month())!
  let periodDate: Date = new Date(year, month, 1)

  _esfService.create(periodDate)
  .then(res => {
    let data = res.data
    if(data.session && data.action){
      index()
      notification.success({
        message: 'Periodo creado'
      }
      )
    }else{
      notification.error({
        message: 'No se creó el periodo',
        description: data.message
      })
    }
  })
  .catch(error => {
    notification.error({
      message: 'Error de conexión',
      description: error.message
    })
  })
}

const openAddFiles = (id : string, key: string) =>{
  let splited = key.split('/')
  let period = meses[parseInt(splited[1]) - 1] + ' ' + String(splited[2].split(' ')[0])

  selectedPeriodo.value.id = id
  selectedPeriodo.value.name = period

  addFiles.value = true
}
</script>

