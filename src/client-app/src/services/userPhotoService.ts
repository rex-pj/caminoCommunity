import { apiConfig } from "../config/api-config";
import axios from "axios";
import BaseService from "./baseService";

const UserPhotoService = class extends BaseService {
  updateAvatar = async (request: any) => {
    return axios
      .put(apiConfig.paths.userPhotos.put.updateAvatar, request, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((response) => {
        return response;
      })
      .catch((error) => {
        console.log(error);
      });
  };

  deleteAvatar = async () => {
    return axios
      .delete(apiConfig.paths.userPhotos.delete.deleteAvatar)
      .then((response) => {
        return response;
      })
      .catch((error) => {
        console.log(error);
      });
  };

  updateCover = async (request: any) => {
    return axios
      .put(`${apiConfig.paths.userPhotos.put.updateCover}`, request)
      .then((response) => {
        return response;
      })
      .catch((error) => {
        console.log(error);
      });
  };

  deleteCover = async () => {
    return axios
      .delete(`${apiConfig.paths.userPhotos.delete.deleteCover}`)
      .then((response) => {
        return response;
      })
      .catch((error) => {
        console.log(error);
      });
  };
};

export default UserPhotoService;
