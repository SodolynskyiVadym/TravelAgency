import { createRouter, createWebHistory } from 'vue-router';
import MainPage from '@/../src/components/MainPage.vue';
import CreateHotel from '@/components/CreatePages/CreateHotel.vue';
import CreateTransport from '@/components/CreatePages/CreateTransport.vue';
import CreateTour from '@/components/CreatePages/CreateTour.vue';
import CreatePlace from '@/components/CreatePages/CreatePlace.vue';
import TourPage from '@/../src/components/TourPage.vue';
import UpdateTourPage from '@/components/UpdatePages/UpdateTour.vue';
import UpdateHotelPage from '@/components/UpdatePages/UpdateHotel.vue';
import UpdateTransportPage from '@/components/UpdatePages/UpdateTransport.vue';
import UpdatePlacePage from '@/components/UpdatePages/UpdatePlace.vue';
import HotelListPage from '@/components/Lists/ListHotels.vue';
import TransportListPage from '@/components/Lists/ListTransports.vue';
import ListUnavailableTours from '@/components/Lists/ListUnavailableTours.vue';
import PlaceListPage from '@/components/Lists/ListPlaces.vue';
import LoginPage from '@/components/Auth/LoginPage.vue';
import RegistrationPage from '@/components/Auth/RegistrationPage.vue';
import UserPage from '@/components/UserPage.vue';
import AdminPage from '@/components/AdminPage.vue';
import CreateUserPage from '@/components/CreatePages/CreateUser.vue';
import ForgotPasswordPage from '@/components/Auth/ForgotPasswordPage.vue';

import * as userAPI from '@/services/API/userAPI';


// import * as listURL from "@/js/listUrl";


const routes = [
    {
        path: '/',
        name: 'MainPage',
        component: MainPage,
    },
    {
        path: '/createHotel',
        name: 'CreateHotelPage',
        component: CreateHotel,
        meta: {
            requiresAuth: true,
            roles: ["ADMIN", "EDITOR"]
        }
    },
    {
        path: '/createTransport',
        name: 'CreateTransportPage',
        component: CreateTransport,
        meta: {
            requiresAuth: true,
            roles: ["ADMIN", "EDITOR"]
        }
    },
    {
        path: '/createTour',
        name: 'CreateTourPage',
        component: CreateTour,
        meta: {
            requiresAuth: true,
            roles: ["ADMIN", "EDITOR"]
        }
    },
    {
        path: '/createPlace',
        name: 'CreatePlacePage',
        component: CreatePlace,
        meta: {
            requiresAuth: true,
            roles: ["ADMIN", "EDITOR"]
        }
    },
    {
        path: '/tour/:id',
        name: 'TourPage',
        component: TourPage,
    },
    {
        path: '/updateTour/:id',
        name: 'UpdateTourPage',
        component: UpdateTourPage,
        meta: {
            requiresAuth: true,
            roles: ["ADMIN", "EDITOR"]
        }
    },
    {
        path: '/updateHotel/:id',
        name: 'UpdateHotelPage',
        component: UpdateHotelPage,
        meta: {
            requiresAuth: true,
            roles: ["ADMIN", "EDITOR"]
        }
    },
    {
        path: '/updateTransport/:id',
        name: 'UpdateTransportPage',
        component: UpdateTransportPage,
        meta: {
            requiresAuth: true,
            roles: ["ADMIN", "EDITOR"]
        }
    },
    {
        path: '/updatePlace/:id',
        name: 'UpdatePlacePage',
        component: UpdatePlacePage,
        meta: {
            requiresAuth: true,
            roles: ["ADMIN", "EDITOR"]
        }
    },
    {
        path: '/listHotels',
        name: 'HotelListPage',
        component: HotelListPage,
    },
    {
        path: '/listTransports',
        name: 'TransportListPage',
        component: TransportListPage,
    },
    {
        path: '/listPlaces',
        name: 'PlaceListPage',
        component: PlaceListPage,
    },
    {
        path: '/login',
        name: 'LoginPage',
        component: LoginPage,
    },
    {
        path: '/registration',
        name: 'RegistrationPage',
        component: RegistrationPage,
    },
    {
        path: "/admin",
        name: "AdminPage",
        component: AdminPage,
        meta: {
            requiresAuth: true,
            roles: ["ADMIN"]
        }
    },
    {
        path: "/user",
        name: "UserPage",
        component: UserPage,
        meta: {
            requiresAuth: true,
            roles: ["ADMIN", "EDITOR", "USER"]
        }
    },
    {
        path: "/createUser",
        name: "CreateUserPage",
        component: CreateUserPage,
        meta: {
            requiresAuth: true,
            roles: ["ADMIN"]
        }
    },
    {
        path: "/forgotPassword",
        name: "ForgotPasswordPage",
        component: ForgotPasswordPage,
    },
    {
        path: "/listUnavailableTours",
        name: "ListUnavailableTours",
        component: ListUnavailableTours,
        meta: {
            requiresAuth: true,
            roles: ["ADMIN", "EDITOR"]
        }
    }
];


const router = createRouter({
    history: createWebHistory(),
    routes
});



router.beforeEach(async (to, from, next) => {
    if (to.meta.requiresAuth) {
        const token = localStorage.getItem('token');
        if (token) {
            const user = await userAPI.getUserByToken(token);
            if(!user) next('/login');
            const role = user.role;

            if (to.meta.roles.includes(role)) {
                next();
            } else {
                next("/login");
            }
        } else {
            next('/login');
        }
    }else {
        next();
    }
});

export default router;