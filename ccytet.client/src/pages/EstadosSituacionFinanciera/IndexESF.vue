<template>
    <div class="mb-2">
      <span class="text-lg font-bold">Estados de situación financiera</span>
    </div>
    <div class="flex">
        <div class="max-w-[300px] min-w-[220px] border mr-2">
            <div class="p-1 mb-3 mx-auto w-fit flex space-x-2">
              <a-date-picker picker="month" v-model:value="date"/>
              <button class="btn-basic text-sm" @click="logDate">Añadir</button>
            </div>
            <a-tree :show-line="showLine" :show-icon="showIcon" :tree-data="treeData" @select="onSelect">
                <template #title="{ dataRef }">
                    <div class="flex items-center space-x-2 hover:text-blue-500">
                        <div>
                            {{ dataRef.title }}
                        </div>
                        <div v-if="dataRef.level == 0">
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                                stroke="currentColor" class="w-4 h-4">
                                <path stroke-linecap="round" stroke-linejoin="round"
                                    d="M8.625 12a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Zm0 0H8.25m4.125 0a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Zm0 0H12m4.125 0a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Zm0 0h-.375M21 12a9 9 0 1 1-18 0 9 9 0 0 1 18 0Z" />
                            </svg>
                        </div>
                    </div>
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
import { ref, type Ref } from 'vue';
import type { TreeProps } from 'ant-design-vue';
import type { Dayjs } from 'dayjs';
import { EsfService } from '@/services/esf-service';

/*
* SERVICES
*/
let esfService: EsfService = new EsfService()

/*
* INITIALIZATION
*/
let create: Ref<boolean> = ref<boolean>(false)
let date: Ref<Dayjs | undefined> = ref<Dayjs>()

const showLine = ref<boolean>(true);
const showIcon = ref<boolean>(false);
const treeData = ref<TreeProps['treeData']>([
  {
    level: 0,
    title: 'parent 1',
    key: '0-0',
    children: [
      {
        title: 'parent 1-0',
        key: '0-0-0',
      },
      {
        title: 'parent 1-1',
        key: '0-0-1',
      },
      {
        title: 'parent 1-2',
        key: '0-0-2',
      },
    ],
  },
  {
    level: 0,
    title: 'parent 2',
    key: '0-1',
    children: [
      {
        title: 'parent 2-0',
        key: '0-1-0',
      },
    ],
  },
]);

/*
* METHODS
*/
const onSelect: TreeProps['onSelect'] = (selectedKeys, info) => {
  console.log(selectedKeys);
  console.log(info)
};

let height: Ref<number> = ref(window.innerHeight - 158)
window.addEventListener('resize', () => {
    height.value = window.innerHeight - 100
})

const logDate = () => {
  let year: number = date.value!.year()
  let month: number = (date.value?.month())!

  let periodDate: Date = new Date(year, month, 1)

  
}
</script>

