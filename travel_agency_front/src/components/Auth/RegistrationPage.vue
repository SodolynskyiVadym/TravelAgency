<template>
    <div class="container-user-input">
        <div class="form">
            <p class="title">Sign Up</p>
            <input placeholder="Email" class="username input" type="text" v-model="user.email">
            <input placeholder="Password" class="password input" type="password" v-model="user.password">
            <input placeholder="Confirm Password" class="password input" type="password" v-model="confirmPassword">

            <div v-if="message">{{ message }}</div>
            <button class="btn" :disabled="user.password != confirmPassword" type="submit" @click="signUp">Sign
                Up</button>
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
                password: '',
            },
            confirmPassword: '',
            message: ""

        }
    },
    methods: {
        async signUp() {
            this.message = "";
            const data = await userAPI.registerUser(this.user);
            if (data === false) {
                this.message = "You haven't registered"
            } else {
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