<template>
    <form class="space-y-4">
      <div>
        <label for="">Nombre: </label>
        <input type="text" class="border p-1" v-model="nombre">
      </div>

      <div>
        <label for="">Apellidos: </label>
        <input type="text" class="border p-1" v-model="apellidos">
      </div>

      <div>
        <label for="">Matricula: </label>
        <input type="text" class="border p-1" v-model="matricula">
      </div>
  
      <div>
        <label for="">Contraseña: </label>
        <input type="text" class="border p-1" v-model="password">
      </div>
        <button type="button" class="border p-1 text-xs hover:bg-gray-200" @click="create()">
        crear usuario
      </button>
    </form>
    
</template>
  
<script lang="ts" setup>

import { ref, type Ref } from 'vue';
import { AuthService } from '@/services/auth-service';
import type { AxiosResponse } from 'axios';

const _authService = new AuthService()

let password: Ref<string>  = ref('')
let nombre: Ref<string>    = ref('')
let matricula: Ref<string> = ref('')
let apellidos: Ref<string> = ref('')

const create = () => {
  _authService.create({
    nombres: nombre.value,
    apellidos: apellidos.value,
    password: password.value,
    matricula: matricula.value
  }).then(res => console.log(res))
}
</script>