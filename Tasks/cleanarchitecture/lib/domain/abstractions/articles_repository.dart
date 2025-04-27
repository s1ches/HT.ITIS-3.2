import '../model/article.dart';

abstract class ArticlesRepository {
  Future<Article> getArticle(int id);
  Future<List<Article>> getArticles({int start, int limit});
}