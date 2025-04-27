import 'package:dio/dio.dart';
import 'model/api_article.dart';

class ArticleApiService {
  final Dio dio;

  ArticleApiService(this.dio);

  Future<ApiArticle> getArticle(int id) async {
    final response = await dio.get('https://jsonplaceholder.typicode.com/posts/$id');
    return ApiArticle.fromJson(response.data);
  }

  Future<List<ApiArticle>> getArticles({int start = 0, int limit = 10}) async {
    final response = await dio.get(
      'https://jsonplaceholder.typicode.com/posts?_start=$start&_limit=$limit',
    );
    final List data = response.data;
    return data.map((json) => ApiArticle.fromJson(json)).toList();
  }
}