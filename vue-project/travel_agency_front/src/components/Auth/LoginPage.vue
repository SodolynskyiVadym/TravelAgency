<template>
    <div class="container-user-input">
        <div class="form">
            <p class="title">Login</p>
            <input placeholder="Email" class="username input" type="text" v-model="user.email" @input="checkEmail">
            <input placeholder="Password" class="password input" type="password" v-model="user.password">
            <div style="cursor: pointer;" class="username input" @click="forgotPassword">Forgot password</div>
            <button class="btn" type="submit" @click="login" v-if="isCorrectEmail && user.password.length >= 8">Login</button>
        </div>
    </div>
</template>

<script>
import * as userAPI from '@/services/API/userAPI';
import * as validEmail from '@/js/validEmail';

export default {
    data() {
        return {
            user: {
                email: '',
                password: ''
            },
            isCorrectEmail: false

        }
    },
    methods: {
        async checkEmail() {
            this.isCorrectEmail = await validEmail.isValidEmail(this.user.email);
        },

        async login() {
            const data = await userAPI.login(this.user);
            if (data) {
                const token = data.token;
                localStorage.setItem('token', token);
                this.$router.push('/');
                setTimeout(() => { window.location.reload(); }, 10);
            }else{
                alert('Invalid email or password');
            }
        },

        async forgotPassword() {
            this.$router.push('/forgotPassword');
        }
    }
}
</script>

<style>
@import "./../../assets/css/styleLoginSignup.css";
</style>