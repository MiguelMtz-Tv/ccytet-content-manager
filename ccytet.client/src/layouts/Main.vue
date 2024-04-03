<template>
<div class="w-full h-screen relative flex">
  <Header></Header>
 
  <div class="overflow-auto mt-[45.8px] bg-white border-r">
    <Sidebar></Sidebar>
  </div>
  
  <!-- Contenido principal -->
  <div class="p-6 overflow-auto flex-grow mt-[45.8px]">
    <slot></slot>
  </div>
</div>
    
</template>

<script setup lang="ts">

import Sidebar from './Sidebar.vue';
import Header from './Header.vue';

</script>