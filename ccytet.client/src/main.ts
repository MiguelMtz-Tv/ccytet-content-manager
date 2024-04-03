import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import router from './routing'
import './styles.css'
import Antd from 'ant-design-vue'
import App from './App.vue'
import CKEditor from '@ckeditor/ckeditor5-vue'

const app = createApp(App)

app.use(CKEditor)
app.use(router)
app.use(Antd)

app.mount('#app')

