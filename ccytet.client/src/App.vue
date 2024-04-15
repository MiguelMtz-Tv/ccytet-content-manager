<script setup lang="ts">
import 'ant-design-vue/dist/reset.css'
import Main from './layouts/Main.vue';
import router from './routing';
import { Sessions } from './libraries/sessions';
import { ref, type Ref } from 'vue';

let auth: Ref<boolean> = ref(false) 

router.beforeEach((to) => {
  auth.value = Sessions.check()
})

</script>

<template>
  <Main v-if="auth">
    <router-view></router-view>
  </Main>

  <router-view v-if="!auth">
  </router-view>
</template>