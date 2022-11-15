import PageSchedule from '../screens/schedule/PageSchedule.vue'
import PageEstablish from '../screens/establish/PageEstablish.vue'
import ScreenAuthority from '../screens/establish/authori/ScreenAuthority.vue'
import ScreenUser from '../screens/establish/user/ScreenUser.vue'
import ScreenUserGroup from '../screens/establish/userGroup/ScreenUserGroup.vue'
const routers = [
    {path: "/schedule", component: PageSchedule},
    {path: "/mission", component: PageSchedule},
    {path: "/transport", component: PageSchedule},
    {path: "/advance", component: PageSchedule},
    {path: "/pay", component: PageSchedule},
    {
        path: "/establish/",
        component: PageEstablish,
        children: [
            {path: "/", component: ScreenUser},
            {path: "user", component: ScreenUser},
            {path: "user-group", component: ScreenUserGroup},
            {path: "authority", component: ScreenAuthority},
            {path: "digital-sign", component: ScreenAuthority},
        ]
    },
]
export default routers;