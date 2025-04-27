class ApiArticle {
  final int userId;
  final int id;
  final String title;
  final String body;

  ApiArticle({required this.userId, required this.id, required this.title, required this.body});

  factory ApiArticle.fromJson(Map<String, dynamic> json) {
    return ApiArticle(
      userId: json['userId'],
      id: json['id'],
      title: json['title'],
      body: json['body'],
    );
  }
}