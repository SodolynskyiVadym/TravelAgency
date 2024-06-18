<template>
    <div class="container-user-input">
        <div class="form">
            <p class="title">Forgot password</p>
            <input placeholder="Email" class="username input" type="text" v-model="email">
            <input placeholder="Password" class="password input" type="password" v-if="isPasswordSended" v-model="password">
            <button class="btn" type="submit" v-if="!isPasswordSended" @click="createReservePassword" :disabled="!email">Send email</button>
            <button class="btn" type="submit" v-else @click="loginViaReservePassword" :disabled="!password">Login</button>
        </div>
    </div>
</template>

<script>
import * as userAPI from '@/services/API/userAPI';

export default {
    data() {
        return {
            email: '',
            password: '',
            isPasswordSended: false,
        }
    },
    methods: {
        async createReservePassword() {
            console.log(this.email);
            await userAPI.createReservePassword(this.email);
            this.isPasswordSended = true;
            console.log(this.isPasswordSended)
        },


        async loginViaReservePassword() {
            const user = {
                email: this.email,
                password: this.password
            }
            const data = await userAPI.loginViaReservePassword(user);
            if (data) {
                localStorage.setItem('token', data.token);
                this.$router.push('/');
            }
        }
    }
}
</script>

<style>
@import "./../../assets/css/styleLoginSignup.css";
</style>