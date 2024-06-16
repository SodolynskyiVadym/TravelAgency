<template>
    <div v-if="isLoaded">
        <input type="text" v-model="user.email" placeholder="Email">
        <input type="text" v-model="user.role" placeholder="Role">
        <input type="password" v-model="user.password" placeholder="Password">
        <input type="password" v-model="confirmPassword" placeholder="Confrm password">
    </div>

</template>

<script>
import * as userAPI from '@/services/API/userAPI';

export default {
    data() {
        return {
            user: null,
            confirmPassword: "",
            isLoaded: false
        }
    },
    methods: {

    },

    async mounted() {
        const token = localStorage.getItem('token');
        if (token) {
            this.user = userAPI.getUserByToken(this.$route.params.id);
            this.isLoaded = true
        }
        this.$router.push('/error');

    }
}
</script>