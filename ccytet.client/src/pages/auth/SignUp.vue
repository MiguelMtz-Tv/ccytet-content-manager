<template>
  <div class="pt-20">
    <div class="border rounded m-auto w-[45%] max-w-[440px] p-4 shadow-md">
      <form action="" @submit.prevent="create()">
        <span class="text-lg font-bold">Crear usuario</span>
        <p class="text-sm">Llenen cuidadosamente el siguiente formulario para crear un usuario.</p>
        <div class="space-y-4">
          <div class="flex flex-col">
            <label for="">Nombre: </label>
            <input type="text" class="border p-1" v-model="nombre">
          </div>
      
          <div class="flex flex-col">
            <label for="">Apellidos: </label>
            <input type="text" class="border p-1" v-model="apellidos">
          </div>
      
          <div class="flex flex-col">
            <label for="">Correo:</label>
            <input type="text" class="border p-1" v-model="matricula">
          </div>
      
          <div class="flex space-x-1">
            <div class="w-full">
              <label for="">Contraseña: </label>
              <input :type="showPassword" class="border p-1" v-model="password">
            </div>

            <div class="w-full">
              <label for="">Confirmar contraseña: </label>
              <input :type="showPassword " class="border p-1" v-model="confirmPassword">
            </div>
          </div>
        </div>
        
        <div class="flex items-center w-full justify-between my-4">
          <label for="hiddenPass" class="flex items-center">
              <input type="checkbox" id="hiddenPass" @change="onCheck">
              <span class="text-xs ml-2">Mostrar contraseña</span>
            </label>

          <router-link to="/sign-in" class="text-blue-500 text-xs">Iniciar sesión</router-link>
        </div>

        <button type="submit" class="btn-primary" :disable="signUpLoading">
          <a-spin v-if="signUpLoading" class="mr-1"/>
          <span v-if="!signUpLoading">crear usuario</span>
        </button>
      </form>
    </div>
  </div>
</template>
  
<script lang="ts" setup>

import { ref, type Ref } from 'vue';
import { AuthService } from '@/services/auth-service';
import type { AxiosResponse } from 'axios';
import {notification} from 'ant-design-vue';
import router from '@/routing';

const _authService = new AuthService()

let password: Ref<string>         = ref('')
let confirmPassword: Ref<string>  = ref('')
let nombre: Ref<string>           = ref('')
let matricula: Ref<string>        = ref('')
let apellidos: Ref<string>        = ref('')

let signUpLoading: Ref<boolean> = ref(false)
let showPassword : Ref<string> = ref('password')

const onCheck = (e : any) => e.target.checked ? showPassword.value = 'text' : showPassword.value = 'password' 

const create = () => {
  if(validateForm()){
    signUpLoading.value = true
    _authService.create({
      nombres: nombre.value,
      apellidos: apellidos.value,
      password: password.value,
      matricula: matricula.value
    })
    .then((res : AxiosResponse) => {
      signUpLoading.value = false
      if(res.status === 200){
        notification.success({message: 'Usuario creado', description: 'Puedes iniciar sesión con el usuario creado'})
        router.push('/sign-in')
      }else{
        notification.error({message: 'Error al crear el usuario', description: res.data})
      }
    })
    .catch(error => {
      signUpLoading.value = false
      notification.error({message: 'Error al crear el usuario', description: error})
    })
  }
}

function errorNotification(message: string) {
  notification.open({
    message: message, placement: 'bottomRight', style: {
      'border': 'red solid 1px',
      'background': '#fff2f0',
    }
  })
}

function validateForm(): boolean{
  let validations : Array<{name : string, valid : boolean, message: string}> = [
    {name: 'nombre',      valid: nombre.value.trim() != '',                                 message: 'Falta el nombre'},
    {name: 'apellidos',   valid: apellidos.value.trim() != '',                              message: 'Faltan los apellidos'},
    {name: 'contraseña',  valid: password.value.trim() != '' && password.value.length >= 8, message: 'Ingresa una contraseña valida (max. 8 caracteres)'},
    {name: 'confirm',     valid: password.value === confirmPassword.value,                  message:'Las contraseñas no coinciden'},
    {name: 'matricula',   valid: matricula.value.trim() != '',                              message: 'La matricula es un campo necesario'}
  ]

  validations.forEach(e => {
    if(!e.valid){
      errorNotification(e.message)
    }
  });

  return validations.every(e => e.valid)
}
</script>
