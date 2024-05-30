<template>
    <v-dialog v-model="addFiles" width="500px">
      <AddFileESF :id="selectedPeriodo.id" :periodo="selectedPeriodo.name" @close-dialog="addFiles = false; onDialogClose($event)"></AddFileESF>
    </v-dialog>
    <div class="mb-2">
      <span class="text-lg font-bold">Estados de situación financiera</span>
    </div>
    <div class="flex">
        <div class="w-[300px] min-w-[220px] border mr-2 overflow-auto resize-x overflow-x-hidden">
            <div class="p-1 mb-3 w-full flex space-x-2 border-b">
              <a-date-picker picker="year" v-model:value="date" @change="index($event.year())"/>
              <select class="text-xs text-center border rounded border-gray-200 px-2" v-model="selectedMonth">
                <option v-for="(item, index) in meses" :value="index">{{ item }}</option>
              </select>
              <button class="btn-basic text-sm" @click="btnCreate()" :disabled="addingESF">Añadir</button>
            </div>
            <a-tree :show-icon="false" :tree-data="treeData" @select="onSelect">
              <template #icon="{ dataRef }">
                <template v-if="dataRef.level === 0 && dataRef.children.length === 0">
                  <div class="relative">
                    <FolderOutlined class="absolute right-1 top-1.5"/>
                  </div>
                </template>
              </template>
              <template #title="{ dataRef }">
                  <a-dropdown :trigger="['contextmenu']" v-if="dataRef.level === 0">
                    <a @click.prevent>
                      <div class="truncate">
                        {{ dataRef.title }}
                      </div>
                    </a>
                    <template #overlay>
                      <a-menu>
                        <a-menu-item @click="openAddFiles(dataRef.id, dataRef.key)">
                          <a>Añadir documento</a>
                        </a-menu-item>
                        <a-menu-item @click="btnDeleteESF(dataRef.id)">
                          <a>Eliminar</a>
                        </a-menu-item>
                      </a-menu>
                    </template>
                  </a-dropdown>
                  <a-dropdown :trigger="['contextmenu']" v-else>
                    <a @click.prevent>
                      <div class="truncate">
                        {{ dataRef.title }}
                      </div>
                    </a>
                    <template #overlay>
                      <a-menu class="max-w-[160px]">
                        <a-menu-item @click="btnDeleteFile(dataRef.id)">
                          <a>Eliminar documento</a>
                        </a-menu-item>
                      </a-menu>
                    </template>
                  </a-dropdown>
                </template>
              </a-tree>
            </div>
            <contextHolder/>
        <iframe class="border p-2 rounded bg-gray-100" :src="baseUrl+selectedPath" width="100%"
            :height="height+'px'">
            Este navegador no soporta iframes.
        </iframe>
    </div>
</template>
<script lang="ts" setup>
import { ref, type Ref, onMounted, h  } from 'vue';
import { notification, type TreeProps } from 'ant-design-vue';
import dayjs, { Dayjs } from 'dayjs';
import { EsfService } from '@/services/esf-service';
import { ExclamationCircleOutlined, FolderOutlined } from '@ant-design/icons-vue';
import AddFileESF from './AddFileESF.vue';
import { Server } from '@/libraries/servers';
import { Modal } from 'ant-design-vue';

/*
* SERVICES
*/
let _esfService: EsfService = new EsfService()
let selectedPeriodo: Ref<{id: string, name: string}> = ref({id: '', name: ''})
let [modal, contextHolder] = Modal.useModal()

/*
* INITIALIZATION
*/
const currentYear: number = new Date().getFullYear() 
let addFiles: Ref<boolean> = ref<boolean>(false)
let date: Ref<Dayjs | undefined> = ref<Dayjs>(dayjs(`${currentYear}/01/01`))

let addingESF: Ref<boolean> = ref<boolean>(false)

const showLine = ref<boolean>(true);
let treeData = ref<TreeProps['treeData']>([]);
let meses: Array<string> = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre']
let selectedMonth: Ref<number> = ref(0)

let selectedPath: Ref<string> = ref('file.pdf')
let baseUrl: string = Server.baseUrl

/*
* METHODS
*/
onMounted(() => {
  index(new Date().getFullYear())
})

const index = (year : number) => {
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
  addingESF.value = true

  let year: number = date.value!.year()
  let month: number = selectedMonth.value
  let periodDate: Date = new Date(year, month, 1)

  _esfService.create(periodDate)
  .then(res => {
    let data = res.data
    if(data.session && data.action){
      index(year)
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
    addingESF.value = false
  })
  .catch(error => {
    notification.error({
      message: 'Error de conexión',
      description: error.message
    })
    addingESF.value = false
  })
}

const openAddFiles = (id : string, key: string) =>{
  let splited = key.split('/')
  let period = meses[parseInt(splited[1]) - 1] + ' ' + String(splited[2].split(' ')[0])

  selectedPeriodo.value.id = id
  selectedPeriodo.value.name = period

  addFiles.value = true
}

const onDialogClose = (e : any) => {
  if(e.created){
    index(e.year)
  }
}

const onSelect: TreeProps['onSelect'] = (selectedKeys, info) => {
  if(info.node.level === 1){
    selectedPath.value = String(info.node.key);
  }
};

const btnDeleteFile = (id : string) => {
  showDeleteFileConfirm(id)
}

const showDeleteFileConfirm = (id : string) => {
  modal.confirm({
    title: '¿Estas seguro que deseas eliminar este archivo?',
    icon: h(ExclamationCircleOutlined),
    content: 'Esta acción quitará el archivo permanentemente de su lugar en la nube',
    okText: 'Eliminar',
    okType: 'danger',
    cancelText: 'Cancelar',
    onOk() {
      deleteFile(id)
    },
    onCancel() {
    },
  });
};

const deleteFile = (id : string) => {
  _esfService.deleteFile(id)
  .then(res => {
    let data = res.data
    if(data.session && data.action){
      try{
        index(date.value!.year())
      }catch{
        index(new Date().getFullYear())
      }
      selectedPath.value = 'file.pdf'
    }else{
      notification.error({
      message: 'No se pudo eliminar el archivo',
      description: data.message
    })
    }
  })
  .catch(error => {
    notification.error({
      message: 'No se pudo eliminar el archivo',
      description: error.message
    })
  }
  )
}

const btnDeleteESF = (id : string) => {
  showDeleteESFConfirm(id)
}

const showDeleteESFConfirm = (id : string) => {
  modal.confirm({
    title: '¿Estas seguro que deseas eliminar este indice estados financieros?',
    icon: h(ExclamationCircleOutlined),
    content: 'No podrás eliminarlo mientras este contenga archivos',
    okText: 'Eliminar',
    okType: 'danger',
    cancelText: 'Cancelar',
    onOk() {
      deleteESF(id)
    },
    onCancel() {},
  });
}

const deleteESF = (id : string) => {
  _esfService.delete(id)
  .then(res => {
    let data = res.data
    if(data.session && data.action){
      try{
        index(date.value!.year())
      }catch{
        index(new Date().getFullYear())
      }
      selectedPath.value = 'file.pdf'
    }else{
      notification.error({
      message: 'No se pudo eliminar el indice',
      description: data.message
    })
    }
  })
  .catch(error => {
    notification.error({
      message: 'No se pudo eliminar el indice',
      description: error.message
    })
  }
  )
}
</script>

