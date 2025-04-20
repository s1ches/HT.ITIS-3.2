import 'package:flutter/material.dart';
import 'package:fluttertoast/fluttertoast.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Calculator',
      theme: ThemeData(
        primarySwatch: Colors.green,
        textTheme: const TextTheme(
          bodyLarge: TextStyle(fontSize: 24, fontWeight: FontWeight.bold),
        ),
        elevatedButtonTheme: ElevatedButtonThemeData(
          style: ElevatedButton.styleFrom(
            backgroundColor: Colors.lightGreen,
            foregroundColor: Colors.white,
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.all(Radius.circular(8)),
            ),
            padding: EdgeInsets.symmetric(vertical: 20),
            textStyle: TextStyle(fontSize: 20),
          ),
        ),
      ),
      home: const CalculatorScreen(),
    );
  }
}

class CalculatorScreen extends StatefulWidget {
  const CalculatorScreen({super.key});

  @override
  State<CalculatorScreen> createState() => _CalculatorScreenState();
}

class _CalculatorScreenState extends State<CalculatorScreen> {
  String expression = '';
  String result = '';
  bool hasError = false;

  void _onPressed(String value) {
    setState(() {
      if (value == '=') {
        try {
          if (expression.contains('/0')) {
            Fluttertoast.showToast(msg: 'Ты чо дурак? Так делать низя!');
            result = '';
            hasError = true;
          } else {
            final parsedResult = _calculate(expression);
            result = parsedResult.toString();
            hasError = false;
          }
        } catch (_) {
          result = 'Ошибка';
          hasError = true;
        }
      } else if (value == 'Clear') {
        expression = '';
        result = '';
        hasError = false;
      } else {
        expression += value;
      }
    });
  }

  num _calculate(String expr) {
    if (expr.contains('+')) {
      final parts = expr.split('+');
      return int.parse(parts[0]) + int.parse(parts[1]);
    } else if (expr.contains('-')) {
      final parts = expr.split('-');
      return int.parse(parts[0]) - int.parse(parts[1]);
    } else if (expr.contains('*')) {
      final parts = expr.split('*');
      return int.parse(parts[0]) * int.parse(parts[1]);
    } else if (expr.contains('/')) {
      final parts = expr.split('/');
      return double.parse(parts[0]) / double.parse(parts[1]);
    }
    return 0;
  }

  Widget _buildButton(String label, {Color? color}) {
    final isEqualOrClear = label == '=' || label == 'Clear';
    return Expanded(
      child: Padding(
        padding: const EdgeInsets.all(4.0),
        child: ElevatedButton(
          onPressed: () => _onPressed(label),
          style: ElevatedButton.styleFrom(
            backgroundColor:
                isEqualOrClear ? Colors.red : color ?? Colors.lightGreen,
          ),
          child: Text(label),
        ),
      ),
    );
  }

  Widget _buildButtonRow(List<String> labels) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
      children: labels.map((e) => _buildButton(e)).toList(),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      body: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 32),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          children: [
            Container(
              width: double.infinity,
              padding: const EdgeInsets.all(12),
              decoration: BoxDecoration(
                color: Colors.grey.shade300,
                borderRadius: BorderRadius.circular(12),
              ),
              child: Text(
                expression,
                style: Theme.of(context).textTheme.bodyLarge,
                textAlign: TextAlign.left,
              ),
            ),
            const SizedBox(height: 16),
            Text(
              result,
              style: TextStyle(
                fontSize: 24,
                color: hasError ? Colors.red : Colors.black,
                fontWeight: hasError ? FontWeight.bold : FontWeight.normal,
              ),
            ),
            const SizedBox(height: 32),
            _buildButtonRow(['1', '2', '3', '+']),
            _buildButtonRow(['4', '5', '6', '-']),
            _buildButtonRow(['7', '8', '9', '*']),
            _buildButtonRow(['0', '=', '/']),
            _buildButtonRow(['Clear']),
          ],
        ),
      ),
    );
  }
}
