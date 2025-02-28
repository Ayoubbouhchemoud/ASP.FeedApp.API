<script setup>
import { ref } from "vue";
import axios from "axios";
import { useRouter } from "vue-router";

const username = ref("");
const password = ref("");
const errorMessage = ref("");
const successMessage = ref("");
const router = useRouter();

const submitForm = async () => {
  errorMessage.value = ""; 
  successMessage.value = "";

  try {
    console.log("Attempting to log in...");

    const response = await axios.post(
      "https://localhost:7245/api/users/login",
      {
        username: username.value,
        password: password.value,
      },
      {
        headers: { "Content-Type": "application/json" },
      }
    );

    if (response.status === 200 && response.data.token) {
      console.log("Login successful:", response.data);

      localStorage.setItem("token", response.data.token);

      successMessage.value = "Login successful! Redirecting...";
      
      setTimeout(() => {
        router.push("/");
      }, 2000);
    } else {
      errorMessage.value = "Unexpected server response.";
    }
  } catch (error) {
    console.error("Login error:", error);

    if (error.response) {
      errorMessage.value = error.response.data?.message || "Login failed.";
    } else if (error.request) {
      errorMessage.value = "The server is not responding. Check your connection.";
    } else {
      errorMessage.value = "An unknown error occurred.";
    }
  }
};
</script>

<template>
  <div class="form-container">
    <div class="form">
      <div class="pp-icon">
        <UnknownProfile></UnknownProfile>
      </div>
      <form @submit.prevent="submitForm">
        <div class="fields">
          <div class="field">
            <label for="username">Benutzername:</label>
            <input type="text" id="username" v-model="username" required />
          </div>

          <div class="field">
            <label for="password">Passwort:</label>
            <input type="password" id="password" v-model="password" required />
          </div>

        </div>
        <button class="btn-submit" type="submit">Login</button>
        <button class="btn-login"><a href="register">Zur Registerung</a></button>
      </form>
      <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
    </div>
  </div>
</template>

<style lang="scss" scoped>
@import '@/assets/css/theme';

.pp-icon {
  position: absolute;
  top: -40%;
  left: 50%;
  transform: translate(-50%, 40%);
  border-radius: 100%;
  box-shadow: inset -5px -5px 9px RGBA(1, 73, 124, 0.35), inset 5px 5px 9px RGBA(1, 73, 124, 0.4);
}

.form {
  margin: 0;
  position: relative;
  //position: absolute;
  //top: 50%;
  //left: 50%;
  //transform: translate(-50%, -50%);

  background: linear-gradient(145deg, #014270, #014e85);
  box-shadow:  25px 25px 37px #012b49,
  -25px -25px 37px #0167af;

  width: 540px;
  height: 580px; // 680px

  background: $layer_color_1;
  border-radius: 1.5rem;

  &-container {
    position: relative;
  }

  form {
    position: relative;
    display: flex;
    flex-direction: column;

    padding: 1rem;

  }
}

.fields {
  width: 80%;
  margin: 10rem auto 0;

  .field {
    display: flex;
    flex-direction: column;
    margin-bottom: 1rem;

    label {
      font-size: 1.25rem;
      margin: 0.25rem 0.5rem;
    }

    input {
      background: $input_fill;
      border: $input_border solid 1px;
      border-radius: 4px;
      min-height: 3rem;
      //box-shadow: inset 0.2rem 0.2rem 0.5rem #bec8e4, inset -0.2rem -0.2rem 0.5rem #fff;
      //box-shadow: inset 0 0 1rem #bec8e4, inset 0 0 10rem #fff;

      padding-left: 1rem;
      padding-right: 1rem;

      &:focus {
        outline: none;
        // #c8d0e7
        box-shadow: inset 0 0 1rem #c8d0e7, inset 0 0 1rem #bec8e4;
      }
    }

  }
}

.btn {
  &-login {
    width: 50%;
    min-height: 1.75rem;
    margin: 1rem auto 0;
    border: 0;
    background: $btn_secondary;
    border-radius: 4px;
    box-shadow: inset 2px 2px 4px #dd5c00,
    inset -2px -2px 4px #ff7600;

    font-size: 1.125rem;
    color: $white;
  }
  &-submit {
    width: 70%;
    min-height: 3.5rem;

    margin: 1rem auto 0.5rem;
    background: $btn_primary;

    border: 0;
    border-radius: 4px;
    box-shadow: inset 2px 2px 4px #dd5c00,
    inset -2px -2px 4px #ff7600;

    font-size: 1.5rem;
    color: $white;
  }
}

.error {
  color: $error_color;
}

a {
  color: $white;
  text-decoration: none;
}

</style>
