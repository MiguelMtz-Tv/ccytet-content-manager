<template>
    <a-menu
          id="dddddd"
          style="width: 256px"
          v-model:openKeys="openKeys"
          v-model:selectedKeys="selectedKeys"
          mode="inline"
        >
          <a-sub-menu key="sub1">
            <template #icon>
                <AppstoreOutlined />
            </template>
            <template #title>Noticias</template>
            <router-link to="/noticias"><a-menu-item key="/noticias">Ver noticias</a-menu-item></router-link>
            <router-link to="/noticias/crear"><a-menu-item key="/noticias/crear">Crear noticia</a-menu-item></router-link>
          </a-sub-menu>

          <a-sub-menu key="sub2">
            <template #icon>
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-4 h-4">
                    <path stroke-linecap="round" stroke-linejoin="round" d="M8.625 9.75a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Zm0 0H8.25m4.125 0a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Zm0 0H12m4.125 0a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Zm0 0h-.375m-13.5 3.01c0 1.6 1.123 2.994 2.707 3.227 1.087.16 2.185.283 3.293.369V21l4.184-4.183a1.14 1.14 0 0 1 .778-.332 48.294 48.294 0 0 0 5.83-.498c1.585-.233 2.708-1.626 2.708-3.228V6.741c0-1.602-1.123-2.995-2.707-3.228A48.394 48.394 0 0 0 12 3c-2.392 0-4.744.175-7.043.513C3.373 3.746 2.25 5.14 2.25 6.741v6.018Z" />
                </svg>
            </template>
            <template #title>Convocatorias</template>
            <router-link to="/convocatorias"><a-menu-item key="/convocatorias">Ver convocatorias</a-menu-item></router-link>
            <router-link to="/convocatorias/crear"><a-menu-item key="/convocatorias/crear">Crear convocatorias</a-menu-item></router-link>
          </a-sub-menu>

          <a-sub-menu key="sub4">
            <template #icon>
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-5 h-5">
                    <path stroke-linecap="round" stroke-linejoin="round" d="M12 6v12m-3-2.818.879.659c1.171.879 3.07.879 4.242 0 1.172-.879 1.172-2.303 0-3.182C13.536 12.219 12.768 12 12 12c-.725 0-1.45-.22-2.003-.659-1.106-.879-1.106-2.303 0-3.182s2.9-.879 4.006 0l.415.33M21 12a9 9 0 1 1-18 0 9 9 0 0 1 18 0Z" />
                </svg>
            </template>
            <template #title>
                <a-tooltip placement="right">
                    <template #title>
                        <span>Estados de situación financiera</span>
                    </template>
                    Estados de situación financiera
                </a-tooltip>
            </template>
            <router-link to="/esf"><a-menu-item key="/esf">Ver Estados</a-menu-item></router-link>
          </a-sub-menu>
        </a-menu>
  </template>
<script setup lang="ts">
  import { ref } from 'vue';
  import { AppstoreOutlined } from '@ant-design/icons-vue';
  const selectedKeys = ref<string[]>([window.location.pathname]);
  const openKeys = ref<string[]>(['sub1']);
</script>