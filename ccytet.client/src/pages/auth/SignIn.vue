<template>
  <div class="container pt-20">
    <div class="border rounded m-auto w-[45%] max-w-[360px] p-4 shadow-md">
      <form @submit.prevent="showData()">
        <span class="text-lg font-bold">Iniciar sesión</span>
        <p class="text-sm">Ingrese su correo y contraseña para iniciar sesión.</p>
        <div class="mb-6 w-full">

          <div class="mb-2">Matricula: </div>
          <input type="text" class="border p-1 mb-4 w-full" v-model="matricula">

          <div class="mb-2">Contraseña: </div>
          <input :type="passwordVisible ? 'text' : 'password'" id="password" class="border p-1 w-full mb-4"
            v-model="password">

          <div class="flex items-center w-full justify-between">
            <label for="hiddenPass" class="flex items-center">
              <input type="checkbox" id="hiddenPass" @change="passwordCheck">
              <span class="text-xs ml-2">Mostrar contraseña</span>
            </label>

            <router-link to="/sign-up" class="text-xs text-blue-500">Crea un usuario</router-link>
          </div>
        </div>
        <button type="submit" class="btn-primary" :disabled="signInLoading">
          <a-spin v-if="signInLoading" class="mr-1" />
          <span v-if="!signInLoading"> Iniciar sesión</span>
        </button>
      </form>
    </div>
  </div>
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

let passwordVisible: Ref<boolean> = ref(false)
const passwordCheck = (e : any) => passwordVisible.value = e.target.checked

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