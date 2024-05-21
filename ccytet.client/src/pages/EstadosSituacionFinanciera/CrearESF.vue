<template>
    <div class="dialog">
        <div class="dialog-header">
            <div>
                <span class="text-lg font-bold">Estados de situación financiera</span> / <span class="text-blue-500">Crear estado de situación financiera</span>
            </div>
        </div>
        <div class="dialog-body">
            <div>
                <a-date-picker picker="month" />
            </div>
        </div>
        <div class="dialog-footer">
            <a-button type="primary" ghost class="mr-2">Crear</a-button>
            <a-button type="primary" ghost @click="emits('close-dialog')">Cerrar</a-button>
        </div>
    </div>
</template>

<script lang="ts" setup>
import { onUnmounted } from 'vue';

const emits = defineEmits(['close-dialog'])

onUnmounted(() => {
    emits('close-dialog')
})

</script>