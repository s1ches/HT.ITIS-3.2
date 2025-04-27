import 'package:cleanarchitecture/domain/abstractions/articles_repository.dart';

import '../../domain/model/article.dart';
import '../api/article/article_api_service.dart';
import '../api/article/article_mapper.dart';

class ArticlesRepositoryImpl extends ArticlesRepository{
  final ArticleApiService _apiService;

  ArticlesRepositoryImpl(this._apiService);

  @override
  Future<Article> getArticle(int id) async {
    final apiArticle = await _apiService.getArticle(id);
    return apiArticle.toDomain();
  }

  @override
  Future<List<Article>> getArticles({int start = 0, int limit = 10}) async {
    final apiArticles = await _apiService.getArticles(start: start, limit: limit);
    return apiArticles.map((e) => e.toDomain()).toList();
  }
}