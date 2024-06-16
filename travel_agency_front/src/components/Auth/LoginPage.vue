<template>
    <div class="container-user-input">
        <div class="form">
            <p class="title">Login</p>
            <input placeholder="Email" class="username input" type="text" v-model="user.email">
            <input placeholder="Password" class="password input" type="password" v-model="user.password">
            <button class="btn" type="submit" @click="login" :disabled="!user.email || user.password < 8">Login</button>
        </div>
    </div>
</template>

<script>
import * as userAPI from '@/services/API/userAPI';

export default {
    data() {
        return {
            user: {
                email: '',
                password: ''
            }

        }
    },
    methods: {
        async login() {
            const data = await userAPI.login(this.user);
            console.log(data);
            const token = data.token;
            if (token) {
                localStorage.setItem('token', token);
                this.$router.push('/');
            }
        }
    }
}
</script>

<style>
@import "./../../assets/css/styleLoginSignup.css";
</style>