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
        <li v-if="user.role === 'ADMIN'" class="nav-link px-2 text-white fs-4" @click="enterAdminPage">Admin</li>
        <li v-if="user.role === 'EDITOR' || user.role === 'ADMIN'" class="nav-link px-2 text-white fs-4"
          @click="enterListPlacesPage">Locations</li>
        <li v-if="user.role === 'EDITOR' || user.role === 'ADMIN'" class="nav-link px-2 text-white fs-4"
          @click="enterListHotelsPage">Hotels</li>
        <li v-if="user.role === 'EDITOR' || user.role === 'ADMIN'" class="nav-link px-2 text-white fs-4"
          @click="enterListTransportsPage">Transports</li>
        <li v-if="user.role === 'EDITOR' || user.role === 'ADMIN'" class="nav-link px-2 text-white fs-4"
          @click="enterListUnavailableToursPage">Unavailable
          Tours</li>
      </ul>

      <div class="text-end" v-if="!user.role">
        <button type="button" class="btn btn-outline-light me-2 fs-5" @click="enterLoginPage">Login</button>
        <button type="button" class="btn btn-primary me-2 fs-5" @click="enterRegistrationPage">Sign-up</button>
      </div>

      <button id="btn-message" class="button-message" @click="enterUserPage" v-if="user.role">
        <div class="content-avatar">
          <div class="status-user"></div>
          <div class="avatar">
            <svg class="user-img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
              <path
                d="M12,12.5c-3.04,0-5.5,1.73-5.5,3.5s2.46,3.5,5.5,3.5,5.5-1.73,5.5-3.5-2.46-3.5-5.5-3.5Zm0-.5c1.66,0,3-1.34,3-3s-1.34-3-3-3-3,1.34-3,3,1.34,3,3,3Z">
              </path>
            </svg>
          </div>
        </div>
        <div class="notice-content">
          <div class="email">{{ user.email }}</div>
          <div class="lable-message">Your page</div>
          <div class="user-id">{{ user.role }}</div>
        </div>
      </button>

      <button class="Btn" v-if="user.role" @click="logout">
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
      user: {
        email: "",
        role: ""
      }
    }
  },

  methods: {
    enterLoginPage() {
      this.$router.push('/login');
    },

    enterRegistrationPage() {
      this.$router.push('/registration');
    },

    async enterUserPage() {
      this.$router.push('/user');
    },

    async enterAdminPage() {
      this.$router.push('/admin');
    },

    async enterListPlacesPage() {
      this.$router.push('/listPlaces');
    },

    async enterListHotelsPage() {
      this.$router.push('/listHotels');
    },

    async enterListTransportsPage() {
      this.$router.push('/listTransports');
    },

    async enterListUnavailableToursPage() {
      this.$router.push('/listUnavailableTours');
    },

    async logout() {
      localStorage.removeItem('token');
      this.role = "";
      window.location.reload();
    }
  },

  async mounted() {
    const token = localStorage.getItem('token');
    if (token) {
      this.user = await userAPI.getUserByToken(token);
      if (!this.user) {
        this.user = {
          role: "",
          email: ""
        }
        localStorage.removeItem('token');
      }
      // localStorage.removeItem('token');
    }
  }
}
</script>

<style>
@import "./assets/css/styleMainPage.css";
@import "./assets/css/styleButtonUserPage.css";
</style>
