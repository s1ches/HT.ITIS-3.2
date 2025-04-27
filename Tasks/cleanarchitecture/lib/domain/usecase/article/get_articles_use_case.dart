import '../../abstractions/articles_repository.dart';
import '../../model/article.dart';

class GetArticlesUseCase {
  final ArticlesRepository _repository;

  GetArticlesUseCase(this._repository);

  Future<List<Article>> execute({int start = 0, int limit = 10}) {
    return _repository.getArticles(start: start, limit: limit);
  }
}