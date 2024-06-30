<template>
    <div class="container-user-input">
        <div class="form">
            <p class="title">Forgot password</p>
            <input placeholder="Email" class="username input" type="text" v-model="user.email" :readonly="isPasswordSended">
            <input placeholder="Password" class="password input" type="password" v-if="isPasswordSended && isPasswordCreated" v-model="user.password">
            <button class="btn" type="submit" v-if="!isPasswordSended && user.email" @click="createReservePassword">Send password</button>
            <button v-if="isPasswordSended && !isPasswordCreated" class="btn" type="button">
                <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
                <span role="status">Loading...</span>
            </button>
            <button class="btn" type="submit" v-if="isPasswordSended && user.password" @click="loginViaReservePassword">Login</button>
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
            },
            isPasswordSended: false,
            isPasswordCreated: false
        }
    },
    methods: {
        async createReservePassword() {
            this.isPasswordSended = true;
            await userAPI.createReservePassword(this.user.email);
            this.isPasswordCreated = true;
        },


        async loginViaReservePassword() {
            console.log(this.user);
            const data = await userAPI.loginViaReservePassword(this.user);
            console.log(data);
            // if (data) {
            //     localStorage.setItem('token', data.token);
            //     this.$router.push('/');
            //     setTimeout(() => { window.location.reload(); }, 10);
            // }
        }
    }
}
</script>

<style>
@import "./../../assets/css/styleLoginSignup.css";
</style>