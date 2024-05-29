<template>
  <header class="p-3 text-bg-dark">
    <div class="d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
      <a href="/" class="d-flex align-items-center mb-2 mb-lg-0 text-white text-decoration-none">
        <svg class="bi me-2" width="40" height="32" role="img" aria-label="Bootstrap">
          <use xlink:href="#bootstrap" />
        </svg>
      </a>

      <ul class="nav col-12 col-lg-auto me-lg-auto mb-2 justify-content-center mb-md-0 ml-auto">
        <li><a href="#" class="nav-link px-2 text-secondary fs-4">Home</a></li>
        <li><a href="#" class="nav-link px-2 text-white fs-4">Features</a></li>
        <li><a href="#" class="nav-link px-2 text-white fs-4">Pricing</a></li>
        <li><a href="#" class="nav-link px-2 text-white fs-4">FAQs</a></li>
        <li><a href="#" class="nav-link px-2 text-white fs-4">About</a></li>
      </ul>

      <div class="text-end">
        <button type="button" class="btn btn-outline-light me-2 fs-5">Login</button>
        <button type="button" class="btn btn-primary me-2 fs-5">Sign-up</button>
      </div>
    </div>
  </header>

  <router-view></router-view>
</template>


<script>

</script>

<style scoped>
@import "./assets/css/styleMain.css";
</style>
