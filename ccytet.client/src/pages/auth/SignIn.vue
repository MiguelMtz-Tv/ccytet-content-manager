<template>
  <form class="space-y-4 flex flex-col items-center justify-center border rounded m-auto w-fit mt-20 p-4 shadow-md">
    <div>
      <div class="mb-2">Matricula: </div>
      <input type="text" class="border p-1" v-model="matricula">
    </div>

    <div>
      <div class="mb-2">Contraseña: </div>
      <input type="password" class="border p-1" v-model="password">
    </div>
    <a-button ghost type="primary" @click="showData()" :loading="signInLoading">
      Iniciar sesión
    </a-button>
  </form>
  
</template>

<script lang="ts" setup>
import { ref, type Ref } from 'vue';
import { AuthService } from '@/services/auth-service';
import type { AxiosResponse } from 'axios';
import { Sessions } from '@/libraries/sessions';
import router from '@/routing';
import { notification } from 'ant-design-vue';

const _authService = new AuthService()

let password: Ref<string> = ref('');
let matricula: Ref<string> = ref('')
let signInLoading: Ref<boolean> = ref(false)

const showData = () => {
  signInLoading.value = true

  _authService.signIn({
    matricula: matricula.value,
    password: password.value
  })
  .catch((reason : any) => {
    return reason.response
  })
  .then(
    (res : AxiosResponse) => {
      signInLoading.value = false
      if(res.status == 200){
        Sessions.start(res.data)
        router.push('/')
      }else{
       notification.error({
        message:'Error',
        description: res.data
      })
      }
    }
  )
}

</script>