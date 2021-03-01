pipeline {
    agent any

    stages {
        stage('GradleBuild') {
            steps {
                sh './java_android/gradlew clean build'
            }
        }
    }
}
