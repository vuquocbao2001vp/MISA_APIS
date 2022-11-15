import { createApp } from 'vue'
import App from './App.vue'
import {createRouter, createWebHistory} from 'vue-router'
import routers from '../src/router/router.js'
import store from '../src/store/index.js'
import DxSelectBox from "devextreme-vue/select-box"
import DxTagBox from "devextreme-vue/tag-box"
import DxCheckBox from "devextreme-vue/check-box"
import DxDropDownBox from 'devextreme-vue/drop-down-box';
import DxTreeView from 'devextreme-vue/tree-view';
import { DxList, DxItemDragging } from 'devextreme-vue/list';
import { DxDataGrid, DxColumn, DxScrolling, DxPaging, DxSelection} from "devextreme-vue/data-grid";
import DxTextBox from 'devextreme-vue/text-box';
import BaseButton from './components/base/BaseButton.vue'
import BasePaging from './components/base/BasePaging.vue'
import BasePopup from './components/base/BasePopup.vue'
import BaseToast from './components/base/BaseToast.vue'
import BaseLoader from './components/base/BaseLoader.vue'

import 'devextreme/dist/css/dx.light.css';

const router = createRouter({
    history: createWebHistory(),
    routes: routers,
})

const app = createApp(App);
app.component("DxSelectBox", DxSelectBox);
app.component("DxTagBox", DxTagBox);
app.component("DxCheckBox", DxCheckBox);
app.component("DxDropDownBox", DxDropDownBox);
app.component("DxTreeView", DxTreeView);
app.component("DxDataGrid", DxDataGrid);
app.component("DxColumn", DxColumn);
app.component("DxScrolling", DxScrolling);
app.component("DxSelection", DxSelection);
app.component("DxPaging", DxPaging);
app.component("DxList", DxList);
app.component("DxItemDragging", DxItemDragging);
app.component("DxTextBox", DxTextBox);
app.component("BaseButton", BaseButton);
app.component("BasePaging", BasePaging);
app.component("BasePopup", BasePopup);
app.component("BaseToast", BaseToast);
app.component("BaseLoader", BaseLoader);


app.use(store);

app.use(router).mount('#app')
