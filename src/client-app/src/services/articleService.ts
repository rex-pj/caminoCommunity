import { apiConfig } from "../config/api-config";
import axios from "axios";
import BaseService from "./baseService";

const ArticleService = class extends BaseService {
  create = async (request: any) => {
    return axios.post(apiConfig.paths.articles.post.postArticle, request, {
      headers: {
        "Content-Type": "multipart/form-data",
        "X-FileUpload-Skip-ContentType": "",
      },
    });
  };

  update = async (request: any, id: number) => {
    let url = `${apiConfig.paths.articles.put.putArticle}`;
    if (id) {
      url += `/${id}`;
    }
    return axios.put(url, request, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
  };

  delete = async (id: number) => {
    return axios.delete(
      `${apiConfig.paths.articles.delete.deleteArticle}/${id}`
    );
  };
};

export default ArticleService;
