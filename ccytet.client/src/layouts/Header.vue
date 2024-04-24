<template>
    <div class="w-full h-[45.8px] fixed border-b flex items-center justify-between bg-white">
        <div class="w-[248.8px] text-center bg-gradient-to-r from-gray-600 to-transparent mt-[-0.5px]">
            <div class="font-bold text-xl text-white">CCYTET</div>
            <div class="text-xs font-semibold text-white">Gestor de contenido</div>
        </div>
        <div class="flex space-x-2 items-center mr-2">
            <a-dropdown :trigger="['click']">
                <a @click.prevent class="ant-dropdown-link cursor-pointer hover:text-blue-400 transition-all duration-300 flex items-center space-x-2 text-gray-600">
                    {{ name }}
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-6 h-6 ml-1.5">
                        <path stroke-linecap="round" stroke-linejoin="round" d="M17.982 18.725A7.488 7.488 0 0 0 12 15.75a7.488 7.488 0 0 0-5.982 2.975m11.963 0a9 9 0 1 0-11.963 0m11.963 0A8.966 8.966 0 0 1 12 21a8.966 8.966 0 0 1-5.982-2.275M15 9.75a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z" />
                    </svg>
                </a>
                <template #overlay>
                  <a-menu>
                    <a-menu-item key="1">
                        <router-link to="/sign-in" @click="logOut()">
                            <div class="flex space-x-2 items-center">
                                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-4 h-4">
                                    <path stroke-linecap="round" stroke-linejoin="round" d="M8.25 9V5.25A2.25 2.25 0 0 1 10.5 3h6a2.25 2.25 0 0 1 2.25 2.25v13.5A2.25 2.25 0 0 1 16.5 21h-6a2.25 2.25 0 0 1-2.25-2.25V15m-3 0-3-3m0 0 3-3m-3 3H15" />
                                </svg>  
                                <span>Cerrar sesión</span>
                            </div>
                        </router-link>
                    </a-menu-item>
                  </a-menu>
                </template>
            </a-dropdown>
        </div>
    </div>
</template>

<script setup lang="ts">
import { Sessions } from '@/libraries/sessions';
import { type Ref, ref } from 'vue';

const logOut = () => {
    Sessions.sessionDestroy()
}

const name: Ref<string> = ref(Sessions.getItem('nombre'))

</script>