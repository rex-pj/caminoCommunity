import { apiConfig } from "../config/api-config";
import axios from "axios";
import BaseService from "./baseService";

const AuthService = class extends BaseService {
  login = async (request) => {
    return axios.post(apiConfig.paths.authentications.post.postLogin, request);
  };

  refreshToken = async () => {
    return axios.post(apiConfig.paths.authentications.post.postRefreshToken);
  };

  updatePassword = async (request) => {
    return axios.patch(
      apiConfig.paths.authentications.patch.patchUpdatePassword,
      request
    );
  };

  forgotPassword = async (request) => {
    return axios.post(
      apiConfig.paths.authentications.post.postForgotPassword,
      request
    );
  };

  resetPassword = async (request) => {
    return axios.post(
      apiConfig.paths.authentications.post.postResetPassword,
      request
    );
  };
};

export default AuthService;
