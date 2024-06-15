<template>
  <header class="p-3 text-bg-dark">
    <div class="d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
      <a href="/" class="d-flex align-items-center mb-2 mb-lg-0 text-white text-decoration-none">
        <svg class="bi me-2" width="40" height="32" role="img" aria-label="Bootstrap">
          <use xlink:href="#bootstrap" />
        </svg>
      </a>

      <ul class="nav col-12 col-lg-auto me-lg-auto mb-2 justify-content-center mb-md-0 ml-auto">
        <li><a href="/" class="nav-link px-2 text-white fs-4">Home</a></li>
        <li><a href="/listPlaces" class="nav-link px-2 text-white fs-4">Locations</a></li>
        <li><a href="/listHotels" class="nav-link px-2 text-white fs-4">Hotels</a></li>
        <li><a href="/listTransports" class="nav-link px-2 text-white fs-4">Transports</a></li>
        <li><a href="/test" class="nav-link px-2 text-white fs-4">Test</a></li>
      </ul>

      <div class="text-end" v-if="!role">
        <button type="button" class="btn btn-outline-light me-2 fs-5" @click="enterLoginPage">Login</button>
        <button type="button" class="btn btn-primary me-2 fs-5" @click="enterRegistrationPage">Sign-up</button>
      </div>

      <button class="Btn" v-if="role" @click="logout">
        <div class="sign"><svg viewBox="0 0 512 512">
            <path
              d="M377.9 105.9L500.7 228.7c7.2 7.2 11.3 17.1 11.3 27.3s-4.1 20.1-11.3 27.3L377.9 406.1c-6.4 6.4-15 9.9-24 9.9c-18.7 0-33.9-15.2-33.9-33.9l0-62.1-128 0c-17.7 0-32-14.3-32-32l0-64c0-17.7 14.3-32 32-32l128 0 0-62.1c0-18.7 15.2-33.9 33.9-33.9c9 0 17.6 3.6 24 9.9zM160 96L96 96c-17.7 0-32 14.3-32 32l0 256c0 17.7 14.3 32 32 32l64 0c17.7 0 32 14.3 32 32s-14.3 32-32 32l-64 0c-53 0-96-43-96-96L0 128C0 75 43 32 96 32l64 0c17.7 0 32 14.3 32 32s-14.3 32-32 32z">
            </path>
          </svg></div>
        <div class="text">Logout</div>
      </button>
    </div>
  </header>

  <router-view style="margin-top: 30px;"></router-view>
</template>


<script>
import * as userAPI from '@/services/API/userAPI';

export default {
  data() {
    return {
      role: ""
    }
  },

  methods: {
    enterLoginPage() {
      this.$router.push('/login');
    },

    enterRegistrationPage() {
      this.$router.push('/registration');
    },

    async logout() {
      localStorage.removeItem('token');
      this.role = "";
    }
  },

  async mounted() {
    const token = localStorage.getItem('token');
    if (token) this.role = await userAPI.getUserRoleByToken(token);
  }
}
</script>

<style>
@import "./assets/css/styleMainPage.css";
</style>
