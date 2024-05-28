import { createRouter, createWebHistory } from 'vue-router';
import MainPage from '@/../src/components/MainPage.vue';

// import * as listURL from "@/js/listUrl";


const routes = [
    {
        path: '/',
        name: 'MainPage',
        component: MainPage,
    }
];


const router = createRouter({
    history: createWebHistory(),
    routes
});



// router.beforeEach(async (to, from, next) => {
//     if (to.meta.requiresAuth) {
//         const token = localStorage.getItem('token');
//         if (token) {
//             const userData = await listURL.getUserByToken(token);
//             const role = userData.role;

//             if (to.meta.roles.includes(role)) {
//                 next();
//             } else {
//                 next("/login");
//             }
//         } else {
//             next('/login');
//         }
//     }else {
//         next();
//     }
// });

export default router;